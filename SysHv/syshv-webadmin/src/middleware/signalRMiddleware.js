import { getUpdate, updateSelectedSensor, addSensor, getSensors } from '../actions/sensor';
import { loginOk, logout } from '../actions/auth';
import { HUB } from '../constants/api';

import { HubConnectionBuilder } from '@aspnet/signalr';

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

  connection.on('sensorAdded', resp => alert(resp));

  console.log(storeAPI.getState().auth.token);

  connection.start().catch(e => storeAPI.dispatch(logout()));

  return next => action => {
    if (action.type === logout().type) {
      connection.stop();
    }

    const res = next(action);

    if (action.type === loginOk().type) {
      connection.stop();
      connection.start();
    }
    if (action.type === addSensor().type) {
      connection.invoke('AddClientSensor', action.payload);
    }

    return res;
  };
};

export default signalRMiddleware;
