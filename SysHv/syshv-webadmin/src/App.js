import React from "react";
import { HubConnectionBuilder, HttpTransportType } from "@aspnet/signalr";
import logo from "./logo.svg";
import "./App.css";

let connection = new HubConnectionBuilder()
  .withUrl("https://localhost:44352/monitoringHub", { 
    accessTokenFactory: () => 'eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEyMyIsIm5iZiI6MTU1NjgyODU2NywiZXhwIjoxNTU2ODcxNzY3LCJpYXQiOjE1NTY4Mjg1NjcsImlzcyI6IkV4YW1wbGVJc3N1ZXIiLCJhdWQiOiJFeGFtcGxlQXVkaWVuY2UifQ.rUYnvWw2gvSiNiUJPtn69hPewO0HqT7rUSRT0cB_vCf6Hs5hRPJcy0AeXsIt9hQHxh4-vZFEf8lIrDYYO7oxWirIaU0SUoyDz1UGmc7EaF7aqQhJDz5eo7V9DnlyzPIQ_BrbQWjGVLiBuNS0n0QaRQXxDFqX2KO6mT46JPzjwN_BR1V5tHZYd_HrNAqF5-StenbAknUjAj4g8VA77LgYoeTSi2lBu7LQDNIJoFLaE2fszH2IBr00s-ab_WohHD8gfvkJr2xdK4F9hBKqznnOWtMKdgFCJvUnvptAnO15_ynkf3gtW47S-uvmL75z0w1z6OFTnQf82kd73h0Ghd6Jpg'
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
