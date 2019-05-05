import { getUpdate } from '../actions/sensor';
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
  });

  if (storeAPI.getState().auth.token) {
    connection.start().catch(e => console.log(e.message));
  }

  return next => action => {
    if (action.type == loginOk().type) {
      connection.start().catch(e => console.log(e.message));
    }

    return next(action);
  };
};

export default signalRMiddleware;
