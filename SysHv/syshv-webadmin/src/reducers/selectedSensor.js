import { createReducer } from 'redux-act';
import * as a from '../actions/sensor';

const DEFAULT_STATE = {
  id: 1,
  values: [],
  subsensors: {}
};

export default createReducer(
  {
    [a.selectSensor]: (state, id) => ({ id, values: [], subsensors: {} }),
    [a.updateSelectedSensor]: (state, update) => {
        console.log(state);
      let values = state.values;
      values.push({
        x: Date.now(),
        y: update.TotalLoad
      });

      let subsensors = state.subsensors;
      for (const key in update.SubSensors) {
          const subsensorName = update.SubSensors[key].CoreName;

          if (!subsensors[subsensorName]) subsensors[subsensorName] = [];
          subsensors[subsensorName].push({
            x: Date.now(),
            y: update.SubSensors[key].Load
          });
      }

      return { id: state.id, values, subsensors };
    }
  },
  DEFAULT_STATE
);
