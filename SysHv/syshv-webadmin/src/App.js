import React from "react";
import { HubConnectionBuilder, HttpTransportType } from "@aspnet/signalr";
import logo from "./logo.svg";
import "./App.css";

let connection = new HubConnectionBuilder()
  .withUrl("https://localhost:44352/monitoringHub", {
    skipNegotiation: true,
    transport: HttpTransportType.WebSockets
  })
  .build();

connection.on("UpdateReceived", data => {
  console.log(data);
});

connection
  .start({ withCredentials: false })
  .catch(err => console.error(err.toString()));

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
