import { createReducer } from 'redux-act'
import * as a from '../actions/auth'

const DEFAULT_STATE = {
  token: localStorage.getItem('token'),
  refreshToken: localStorage.getItem('refreshToken'),
  isSuccessResetPassword: false,
}

export default createReducer({
  [a.loginOk]: (state, { token, refreshToken }) => {
    localStorage.setItem('token', token)
    localStorage.setItem('refreshToken', refreshToken)
    return { ...state, token, refreshToken }
  },
  [a.logout]: (state) => {
    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
    return {
      ...state,
      token: '',
      refreshToken: ''
    }
  }
}, DEFAULT_STATE )