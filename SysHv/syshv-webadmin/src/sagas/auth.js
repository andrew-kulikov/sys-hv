import { put } from 'redux-saga/effects';

import {
  LOGIN,
  REGISTER
} from '../constants/api';

import {
  loginOk,
} from '../actions/auth';

import { callHttp } from '../utils/api';
import { post } from '../utils/httpUtil';
import { toastr } from 'react-redux-toastr';
import * as messageTypes from '../constants/messageTypes';

export function* login({ payload }) {
  const { email, password } = payload;
  try {
    const data = yield callHttp(post, LOGIN, { email, password });

    yield put(
      loginOk({
        token: data.token,
        refreshToken: data.refresh_tokens
      })
    );
  } catch (err) {
    toastr.error(messageTypes.ERROR, 'Authorization failed');
  }
}

export function* register({ payload }) {
  const { email, password } = payload;
  try {
    yield callHttp(post, REGISTER, { email, password });

    toastr.info(messageTypes.SUCCESS, 'Registration completed successfully')
  } catch (err) {
    toastr.error(messageTypes.ERROR, err.message);
  }
}
