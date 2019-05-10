const BACKEND = process.env.REACT_APP_API_URL;
const API = BACKEND + 'api/';

export const HUB = BACKEND + 'monitoringHub';

export const LOGIN = `${API}account/login`;
export const REGISTER = `${API}account/register`;
export const REFRESHTOKEN = `${API}token/refresh`;

export const FORGOTPASSWORD = `${API}forgotpassword`;
export const RESETPASSWORD = `${API}resetpassword`;

export const CLIENTS = `${API}client`;
export const ADD_CLIENT = `${API}client/register`;

export const SENSORS = `${API}sensor`;

export const SENSOR_LOGS = id => `${API}log/sensor/${id}`;
export const ALL_LOGS = `${API}log/last`;

export const CLIENT_SENSOR = id => `${API}sensor/${id}`;

export const ME = `${API}me`;
