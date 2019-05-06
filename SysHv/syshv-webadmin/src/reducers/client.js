import { createReducer } from 'redux-act';
import * as a from '../actions/client';

const DEFAULT_STATE = {
  clients: []
};

export default createReducer(
  {
    [a.setClients]: (state, clients) => ({ ...state, clients })
  },
  DEFAULT_STATE
);
