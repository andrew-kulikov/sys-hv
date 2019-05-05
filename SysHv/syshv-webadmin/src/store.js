import { createStore, combineReducers, applyMiddleware } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
//import { reducer as formReducer } from 'redux-form'

import reducers from './reducers';
import sagaMiddleware from './middleware/sagaMiddleware';
import signalRMiddleware from './middleware/signalRMiddleware';

export default createStore(
  combineReducers(reducers),
  composeWithDevTools(applyMiddleware(sagaMiddleware, signalRMiddleware))
);
