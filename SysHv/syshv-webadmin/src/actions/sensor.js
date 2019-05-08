import { createAction } from 'redux-act';

export const getUpdate = createAction('getUpdate');

export const updateSelectedSensor = createAction('updateSelectedSensor');

export const addSensor = createAction('addSensor');

export const getSensors = createAction('getSensors');
export const setSensors = createAction('setSensors');

export const getClientSensor = createAction('getClientSensor');
export const selectSensor = createAction('selectSensor');
