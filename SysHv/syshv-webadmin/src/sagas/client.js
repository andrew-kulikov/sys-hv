import { put } from 'redux-saga/effects';

import { CLIENTS, ADD_CLIENT } from '../constants/api';

import { callHttp } from '../utils/api';
import { get, post } from '../utils/httpUtil';

import { setClients } from '../actions/client';

export function* getClients() {
  const data = yield callHttp(get, CLIENTS);
  yield put(setClients(data));
}

export function* addClient({ payload }) {
  try {
    const data = yield callHttp(post, ADD_CLIENT, payload);
    console.log('Client added:', data);
  } catch (err) {
    console.log(err);
  }
}
