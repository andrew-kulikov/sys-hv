import { put } from 'redux-saga/effects';

import { SENSORS } from '../constants/api';

import { setSensors } from '../actions/sensor';

import { callHttp } from '../utils/api';
import { get } from '../utils/httpUtil';
import { toastr } from 'react-redux-toastr';
import * as messageTypes from '../constants/messageTypes';


export function* getSensors() {
  const sensors = yield callHttp(get, SENSORS);
  yield put(setSensors(sensors));
}
