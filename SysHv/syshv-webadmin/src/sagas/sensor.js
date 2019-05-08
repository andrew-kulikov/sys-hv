import { put } from 'redux-saga/effects';

import { SENSORS, CLIENT_SENSOR } from '../constants/api';

import { setSensors, selectSensor } from '../actions/sensor';

import { callHttp } from '../utils/api';
import { get } from '../utils/httpUtil';
import { toastr } from 'react-redux-toastr';
import * as messageTypes from '../constants/messageTypes';

export function* getSensors() {
  const sensors = yield callHttp(get, SENSORS);
  yield put(setSensors(sensors));
}

export function* getClientSensor({ payload }) {
  const id = payload;
  const sensor = yield callHttp(get, CLIENT_SENSOR(id));
  yield put(selectSensor(sensor));
}
