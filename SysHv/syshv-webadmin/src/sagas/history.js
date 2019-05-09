import { put } from 'redux-saga/effects';

import { ALL_LOGS, SENSOR_LOGS } from '../constants/api';

import { setHistory } from '../actions/history';

import { callHttp } from '../utils/api';
import { get } from '../utils/httpUtil';
import { toastr } from 'react-redux-toastr';
import * as messageTypes from '../constants/messageTypes';

export function* getLastHistory() {
  const history = yield callHttp(get, ALL_LOGS);
  yield put(setHistory(history));
}

export function* getHistory({ payload }) {
  const clientSensorId = payload;
  const history = yield callHttp(get, SENSOR_LOGS(clientSensorId));
  yield put(setHistory(history));
}
