import { createReducer } from 'redux-act';
import * as a from '../actions/sensor';

const DEFAULT_STATE = {
  sensor: {},
  values: [],
  subsensors: {}
};

const getValue = val => {
  if (+val === NaN || !val) {
    console.warn(val);
    return 0;
  }
  return +val;
};

export default createReducer(
  {
    [a.selectSensor]: (state, sensor) => ({ sensor, values: [], subsensors: {} }),
    [a.updateSelectedSensor]: (state, { update, date }) => {
      //console.log(update, date);
      let values = state.values;

      values.push({
        x: date,
        y: getValue(update.Value)
      });

      if (values.length > 25) values = values.slice(-10);

      let subsensors = state.subsensors;
      for (const key in update.SubSensors) {
        const subsensorName = update.SubSensors[key].Name;

        if (!subsensors[subsensorName]) subsensors[subsensorName] = [];
        subsensors[subsensorName].push({
          x: date,
          y: getValue(update.SubSensors[key].Value)
        });
        if (subsensors[subsensorName].length > 25)
          subsensors[subsensorName] = subsensors[subsensorName].slice(-10);
      }

      return { ...state, values, subsensors };
    }
  },
  DEFAULT_STATE
);
