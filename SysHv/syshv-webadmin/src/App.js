import React from 'react';
import ReduxToastr from 'react-redux-toastr';
import { Route, Switch, BrowserRouter as Router } from 'react-router-dom';

import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Account from './pages/Account';
import Sensors from './pages/Sensors';
import Computers from './pages/Computers';
import Logs from './pages/Logs';
import Sensor from './pages/Sensor';

import GenericNotFound from './pages/GenericNotFound';

import saga from './sagas';
import store from './store';
import { Provider } from 'react-redux';
import { sagaMiddleware } from './middleware/sagaMiddleware';

const App = props => (
  <Provider store={store}>
    <Router>
      <>
        <Switch>
          <Route exact path="/" component={Home} />
          <Route path="/home" component={Home} />
          <Route path="/login" component={Login} />
          <Route path="/register" component={Register} />
          <Route path="/account" component={Account} />
          <Route path="/sensors" component={Sensors} />
          <Route path="/computers" component={Computers} />
          <Route path="/logs" component={Logs} />
          <Route path="/logs" component={Logs} />
          <Route path="/sensor/:id" component={Sensor} />

          <Route component={GenericNotFound} />
        </Switch>
        <ReduxToastr closeOnToastrClick={true} progressBar={true} />
      </>
    </Router>
  </Provider>
);

export default App;
sagaMiddleware.run(saga);
