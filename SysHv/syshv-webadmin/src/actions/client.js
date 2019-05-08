import { createAction } from 'redux-act';

export const getClients = createAction('getClients');
export const setClients = createAction('setClients');

export const addClient = createAction('addClient');
export const deleteClient = createAction('deleteClient');
