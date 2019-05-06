import auth from './auth';
import home from './home';
import account from './account';
import sensor from './sensor';
import client from './client';
import selectedSensor from './selectedSensor';
import { reducer as toastr } from 'react-redux-toastr';

export default {
  auth,
  home,
  account,
  toastr,
  sensor,
  client,
  selectedSensor
};
