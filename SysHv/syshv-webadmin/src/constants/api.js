const BACKEND = process.env.REACT_APP_API_URL
const API = BACKEND + 'api/'

export const HUB = BACKEND + 'monitoringHub'

export const LOGIN = `${API}account/login`
export const REFRESHTOKEN = `${API}token/refresh`

export const FORGOTPASSWORD = `${API}forgotpassword`
export const RESETPASSWORD = `${API}resetpassword`

export const USERS = `${API}my/user`
export const USER = (id) => `${API}my/user/${id}`

export const GROUP_USERS = `${API}my/users`

export const ME = `${API}me`