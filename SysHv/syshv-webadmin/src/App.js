import React from "react";
import { HubConnectionBuilder, HttpTransportType } from "@aspnet/signalr";
import logo from "./logo.svg";
import "./App.css";

let connection = new HubConnectionBuilder()
  .withUrl("https://localhost:44352/monitoringHub", {
    accessTokenFactory: async () => {
      let res = await fetch("https://localhost:44352/api/account/login", {
        method: "post",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          Email: "123",
          Password: "123Qwe!"
        })
      });
      let token = await res.json().then(j =>j.token);
      return token;
    }
  })
  .build();

connection.on("UpdateReceived", data => {
  console.log(data);
});

connection
  .start()
  .then(() => connection.invoke("AddSensor"))
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
