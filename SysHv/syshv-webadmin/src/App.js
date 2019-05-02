import React from 'react';
import { HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';
import logo from './logo.svg';
import './App.css';

import Login from './components/Login';
import Main from './components/Main';

const initHub = () => {
  let connection = new HubConnectionBuilder()
    .withUrl('https://localhost:44352/monitoringHub', {
      accessTokenFactory: async () => {
        let res = await fetch('https://localhost:44352/api/account/login', {
          method: 'post',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            Email: '123',
            Password: '123Qwe!'
          })
        });
        let token = await res.json().then(j => j.token);
        return token;
      }
    })
    .build();

  connection.on('UpdateReceived', data => {
    console.log(data);
  });

  connection.start().catch(err => console.error(err));
};

class App extends React.Component {
  state = {
    logged: false
  };

  login = () => {
    initHub();
    this.setState({ logged: true });
  }

  render() {
    console.log(this.state.logged)
    return this.state.logged ? <Main /> : <Login handleSubmit={this.login} />;
  }
}

export default App;
