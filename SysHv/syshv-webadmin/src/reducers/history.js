import { createReducer } from 'redux-act';
import * as a from '../actions/history';

const DEFAULT_STATE = {
  history: []
};

export default createReducer(
  {
    [a.setHistory]: (state, history) => ({ history })
  },
  DEFAULT_STATE
);
