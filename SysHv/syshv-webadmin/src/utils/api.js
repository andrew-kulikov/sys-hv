import { call, put, take } from 'redux-saga/effects';
import { refreshToken, loginOk } from '../actions/auth';
import jwt_decode from 'jwt-decode';

export function* callHttp(...args) {
  if (process.env.REACT_APP_MOCK) return;

  try {
    const token = localStorage.getItem('token');

    if (token) {
      let decoded;
      try {
        decoded = jwt_decode(token);
      } catch (error) {
        throw new Error(`Invalid token ${token}`);
      }

      if (
        decoded &&
        typeof decoded.exp !== 'undefined' &&
        decoded.exp < new Date().getTime() / 1000
      ) {
        throw new Error('401 Forbidden');
      }
    }

    const data = yield call(...args);
    return data;
  } catch (err) {
    if (err.status === 401) {
      console.log(err);
    }
    throw err;
  }
}