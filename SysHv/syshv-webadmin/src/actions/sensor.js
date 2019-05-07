import { createAction } from 'redux-act';

export const getUpdate = createAction('getUpdate');

export const selectSensor = createAction('selectSensor');
export const updateSelectedSensor = createAction('updateSelectedSensor');

export const addSensor = createAction('addSensor');

export const getSensors = createAction('getSensors');
export const setSensors = createAction('setSensors');