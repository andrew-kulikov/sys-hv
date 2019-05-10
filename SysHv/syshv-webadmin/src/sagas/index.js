import { takeLatest } from 'redux-saga/effects';

import * as authActions from '../actions/auth';
import * as authSagas from './auth';

import * as accountActions from '../actions/account';
import * as accountSagas from './account';

import * as clientActions from '../actions/client';
import * as clientSagas from './client';

import * as sensorActions from '../actions/sensor';
import * as sensorSagas from './sensor';

import * as historyActions from '../actions/history';
import * as historySagas from './history';

export default function* saga() {
  const relations = [
    [authActions, authSagas],
    [accountActions, accountSagas],
    [clientActions, clientSagas],
    [sensorActions, sensorSagas],
    [historyActions, historySagas]
  ];

  for (const [actions, sagas] of relations) {
    for (const [actionName, action] of Object.entries(actions)) {
      const saga = sagas[actionName];

      if (saga) yield takeLatest(action.getType(), saga); // for multiple same async requests running at the same time use takeEvery (e.g. nodes for TreeView loading)
    }
  }
}
