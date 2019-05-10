import React from 'react';

import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import DashboardIcon from '@material-ui/icons/Dashboard';
import LaptopIcon from '@material-ui/icons/Laptop';
import BarChartIcon from '@material-ui/icons/BarChart';
import LayersIcon from '@material-ui/icons/Layers';
import LogoutIcon from '@material-ui/icons/ExitToApp';

import { logout } from '../../actions/auth';
import { connectTo } from '../../utils';

import { withRouter } from 'react-router-dom';

export const MainListItems = withRouter(props => (
  <div>
    <ListItem button onClick={() => props.history.push('/computers')}>
      <ListItemIcon>
        <LaptopIcon />
      </ListItemIcon>
      <ListItemText primary="Computers" />
    </ListItem>
    <ListItem button onClick={() => props.history.push('/sensors')}>
      <ListItemIcon>
        <BarChartIcon />
      </ListItemIcon>
      <ListItemText primary="Sensors" />
    </ListItem>
    <ListItem button onClick={() => props.history.push('/logs')}>
      <ListItemIcon>
        <LayersIcon />
      </ListItemIcon>
      <ListItemText primary="Reports" />
    </ListItem>
  </div>
));

export const SecondaryListItems = connectTo(
  state => ({}),
  {
    logout
  },
  props => (
    <div>
      <ListItem button onClick={() => props.logout()}>
        <ListItemIcon>
          <LogoutIcon />
        </ListItemIcon>
        <ListItemText primary="Logout" />
      </ListItem>
    </div>
  )
);
