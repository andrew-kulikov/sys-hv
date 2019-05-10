import { HubConnectionBuilder } from '@aspnet/signalr';
import { HUB } from '../constants/api';

import { getUpdate, updateSelectedSensor, addSensor } from '../actions/sensor';
import { addNotification } from '../actions/notifications';

import { loginOk, logout } from '../actions/auth';

const signalRMiddleware = storeAPI => {
  let connection = new HubConnectionBuilder()
    .withUrl(HUB, {
      accessTokenFactory: () => storeAPI.getState().auth.token
    })
    .build();

  connection.on('UpdateReceived', message => {
    storeAPI.dispatch(getUpdate(message));

    if (storeAPI.getState().selectedSensor.sensor.id == message.SensorId)
      storeAPI.dispatch(
        updateSelectedSensor({ update: message.Value, date: message.Time })
      );
  });

  connection.on('notificationReceived', notification =>
    storeAPI.dispatch(addNotification(notification))
  );
  connection.on('sensorAdded', resp => console.log(resp));
  connection.on('clientAdded', client => console.log(client));

  connection.start().catch(e => {
    console.log(e.toString());
    storeAPI.dispatch(logout());
  });

  return next => action => {
    if (action.type === logout().type) connection.stop();

    const res = next(action);

    if (action.type === loginOk().type) {
      connection.stop();
      connection.start();
    }

    if (action.type === addSensor().type)
      connection.invoke('AddClientSensor', action.payload);

    return res;
  };
};

export default signalRMiddleware;
