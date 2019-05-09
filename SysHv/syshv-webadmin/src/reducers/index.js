import auth from './auth';
import home from './home';
import account from './account';
import sensor from './sensor';
import client from './client';
import history from './history';
import selectedSensor from './selectedSensor';
import allSensors from './allSensors';
import { reducer as toastr } from 'react-redux-toastr';

export default {
  auth,
  home,
  account,
  toastr,
  sensor,
  client,
  history,
  allSensors,
  selectedSensor
};
