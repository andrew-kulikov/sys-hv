import { createReducer } from 'redux-act';
import * as a from '../actions/sensor';

const DEFAULT_STATE = {
  sensorValue: {}
};

export default createReducer(
  {
    [a.getUpdate]: (state, update) => {
      console.log(update);
      return { ...state, sensorValue: update };
    }
  },
  DEFAULT_STATE
);
