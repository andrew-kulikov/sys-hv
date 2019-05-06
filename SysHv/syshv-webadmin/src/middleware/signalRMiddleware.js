import { getUpdate, updateSelectedSensor } from '../actions/sensor';
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

    if (storeAPI.getState().selectedSensor.id == message.SensorId) 
      storeAPI.dispatch(updateSelectedSensor(message.Value));
    
  });

  connection.on('sensorAdded', () => alert('sensor added'));

  console.log(storeAPI.getState().auth.token);
  if (storeAPI.getState().auth.token) {
    connection.start().catch(e => storeAPI.dispatch(logout()));
  }

  return next => action => {
    const res = next(action);

    if (action.type === loginOk().type) {
      //connection.start().catch(e => console.log(e.message));
    }

    return res;
  };
};

export default signalRMiddleware;
