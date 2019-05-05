import React from 'react';
import ReduxToastr from 'react-redux-toastr';
import { Route, Switch, BrowserRouter as Router } from 'react-router-dom';

import GenericNotFound from './pages/GenericNotFound';

import { sagaMiddleware } from './middleware/sagaMiddleware';
import saga from './sagas';
import { Provider } from 'react-redux';
import store from './store';

import home from './pages/Home';
import logout from './containers/Logout';
import login from './pages/Login';
import myAccount from './pages/Account';
import ResetPass from './pages/ResetPass';

const App = props => (
  <Provider store={store}>
    <Router>
      {/* <MainLayout> */}
      <>
        <Switch>
          <Route exact path="/" component={home} />
          <Route path="/home" component={home} />
          <Route path="/login" component={login} />
          <Route path="/account" component={myAccount} />
          <Route
            path="/password/reset"
            component={props => <ResetPass {...props} type="reset" />}
          />
          <Route
            path="/password/new"
            component={props => <ResetPass {...props} type="new" />}
          />
          <Route path="/logout" component={logout} />
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
