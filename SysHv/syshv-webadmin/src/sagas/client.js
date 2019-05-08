import { put } from 'redux-saga/effects';

import { CLIENTS, ADD_CLIENT } from '../constants/api';

import { callHttp } from '../utils/api';
import { get, post, del } from '../utils/httpUtil';

import { toastr } from 'react-redux-toastr';
import { SUCCESS } from '../constants/messageTypes';
import { setClients } from '../actions/client';

export function* getClients() {
  const data = yield callHttp(get, CLIENTS);
  yield put(setClients(data));
}

export function* addClient({ payload }) {
  try {
    const data = yield callHttp(post, ADD_CLIENT, payload);
    toastr.info(SUCCESS, `Client ${data.name} successfully added`);
  } catch (err) {
    console.log(err);
  }
}

export function* deleteClient({ payload }) {
  try {
    const id = payload;
    const result = yield callHttp(del, CLIENTS, id);

    toastr.info(SUCCESS, `Client ${id} successfully removed`);
  } catch (err) {
    console.log(err);
  }
}
