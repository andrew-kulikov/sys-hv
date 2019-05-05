import { createReducer } from 'redux-act';
import * as a from '../actions/sensor';

const DEFAULT_STATE = {
  sensorValues: {}
};

export default createReducer(
  {
    [a.getUpdate]: (state, update) => {
      //console.log(update);
      let sensorValues = state.sensorValues;
      if (!sensorValues[update.SensorId]) sensorValues[update.SensorId] = [];
      sensorValues[update.SensorId].push({
        x: Date.now(),
        y: update.Value.TotalLoad
      });
      return { ...state, sensorValues };
    }
  },
  DEFAULT_STATE
);
