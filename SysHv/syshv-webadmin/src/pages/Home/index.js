import React from 'react';

import styles from './style';
import Page from '../page';

import { withStyles } from '@material-ui/core/styles';
import { withNamespaces } from 'react-i18next';

import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';

import SimpleLineChart from '../../components/Home/SimpleLineChart';
import SimpleTable from '../../components/Home/SimpleTable';

const HomePage = ({ classes }) => (
  <Page title="Dashboard">
    <Typography variant="h4" gutterBottom component="h2">
      Hello, admin
    </Typography>
    <Paper title="Clients">Clients</Paper>
    <Paper title="Sensors">Sensors</Paper>
    <Paper title="Stats">Stats</Paper>
  </Page>
);

export default withNamespaces()(withStyles(styles)(HomePage));
