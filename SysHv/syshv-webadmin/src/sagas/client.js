import { put } from 'redux-saga/effects';

import { CLIENTS } from '../constants/api';

import { callHttp } from '../utils/api';
import { get } from '../utils/httpUtil';

import { setClients } from '../actions/client';

export function* getClients() {
    console.log('sdfsdf');
  const data = yield callHttp(get, CLIENTS);
  console.log(data);
  yield put(setClients(data));
}
