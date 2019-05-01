import React from "react";
import { HubConnectionBuilder, HttpTransportType } from "@aspnet/signalr";
import logo from "./logo.svg";
import "./App.css";

let connection = new HubConnectionBuilder()
  .withUrl("http://syshv.westeurope.cloudapp.azure.com/monitoringHub")
  .build();

connection.on("UpdateReceived", data => {
  console.log(data);
});

connection
  .start()
  .catch(err => console.error(err));

const App = props => (
  <div className="App">
    <header className="App-header">
      <img src={logo} className="App-logo" alt="logo" />
      <p>
        Edit <code>src/App.js</code> and save to reload.
      </p>
      <a
        className="App-link"
        href="https://reactjs.org"
        target="_blank"
        rel="noopener noreferrer"
      >
        Learn React
      </a>
    </header>
  </div>
);

export default App;
