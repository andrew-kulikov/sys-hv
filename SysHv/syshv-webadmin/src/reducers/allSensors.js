import { createReducer } from 'redux-act';
import * as a from '../actions/sensor';

const DEFAULT_STATE = {
  sensors: []
};

export default createReducer(
  {
    [a.setSensors]: (state, sensors) => ({
      ...state,
      sensors
    })
  },
  DEFAULT_STATE
);
