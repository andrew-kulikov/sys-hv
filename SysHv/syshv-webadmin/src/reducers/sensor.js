import { createReducer } from 'redux-act';
import * as a from '../actions/sensor';

const DEFAULT_STATE = {
  sensorValues: {}
};

export default createReducer(
  {
    [a.getUpdate]: (state, update) => {
      let sensorValues = state.sensorValues;

      if (!sensorValues[update.SensorId]) sensorValues[update.SensorId] = [];

      sensorValues[update.SensorId].push(update);

      if (sensorValues[update.SensorId].length > 25)
        sensorValues[update.SensorId] = sensorValues[update.SensorId].slice(-10);

      return { ...state, sensorValues };
    }
  },
  DEFAULT_STATE
);
