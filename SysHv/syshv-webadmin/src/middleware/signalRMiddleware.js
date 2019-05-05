import { getUpdate } from '../actions/sensor';

import { HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';

const signalRMiddleware = storeAPI => {
  let connection = new HubConnectionBuilder()
    .withUrl('https://localhost:44352/monitoringHub', {
      accessTokenFactory: () => localStorage.getItem('token')
    })
    .build();

  connection.on('UpdateReceived', message => {
    storeAPI.dispatch(getUpdate(message));
  });

  connection.start().catch(err => console.error(err));

  return next => action => {
      /*
    if (action.type == 'SEND_WEBSOCKET_MESSAGE') {
      socket.send(action.payload);
      return;
    }*/

    return next(action);
  };
};

export default signalRMiddleware;
