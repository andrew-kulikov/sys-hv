import { createReducer } from 'redux-act';
import * as a from '../actions/sensor';

const DEFAULT_STATE = {
  id: 2,
  values: [],
  subsensors: {}
};

export default createReducer(
  {
    [a.selectSensor]: (state, id) => ({ id, values: [], subsensors: {} }),
    [a.updateSelectedSensor]: (state, update) => {
      //console.log(update);
      let values = state.values;

      values.push({
        x: Date.now(),
        y: update.Value
      });

      if (values.length > 25) values = values.slice(-10);

      let subsensors = state.subsensors;
      for (const key in update.SubSensors) {
        const subsensorName = update.SubSensors[key].Name;

        if (!subsensors[subsensorName]) subsensors[subsensorName] = [];
        subsensors[subsensorName].push({
          x: Date.now(),
          y: update.SubSensors[key].Value
        });
        if (subsensors[subsensorName].length > 25)
          subsensors[subsensorName] = subsensors[subsensorName].slice(-10);
      }

      return { id: state.id, values, subsensors };
    }
  },
  DEFAULT_STATE
);
