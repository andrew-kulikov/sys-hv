import React from 'react';
import ReduxToastr from 'react-redux-toastr';
import { Route, Switch, BrowserRouter as Router } from 'react-router-dom';

import GenericNotFound from './pages/GenericNotFound';

import { sagaMiddleware } from './middleware/sagaMiddleware';
import saga from './sagas';
import { Provider } from 'react-redux';
import store from './store';

import Home from './pages/Home';
import Logout from './containers/Logout';
import Login from './pages/Login';
import Account from './pages/Account';
import ResetPass from './pages/ResetPass';
import Sensors from './pages/Sensors';
import Computers from './pages/Computers';
import Logs from './pages/Logs';

const App = props => (
  <Provider store={store}>
    <Router>
      {/* <MainLayout> */}
      <>
        <Switch>
          <Route exact path="/" component={Home} />
          <Route path="/home" component={Home} />
          <Route path="/login" component={Login} />
          <Route path="/account" component={Account} />
          <Route path="/sensors" component={Sensors} />
          <Route path="/computers" component={Computers} />
          <Route path="/logs" component={Logs} />
          <Route
            path="/password/reset"
            component={props => <ResetPass {...props} type="reset" />}
          />
          <Route
            path="/password/new"
            component={props => <ResetPass {...props} type="new" />}
          />
          <Route path="/logout" component={Logout} />
          <Route component={GenericNotFound} />
        </Switch>
        <ReduxToastr closeOnToastrClick={true} progressBar={true} />
      </>
      {/* </MainLayout> */}
    </Router>
  </Provider>
);

export default App;
sagaMiddleware.run(saga);
