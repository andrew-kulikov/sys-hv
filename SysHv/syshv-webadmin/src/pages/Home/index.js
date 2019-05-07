import React from 'react';

import styles from './style';
import Page from '../page';
import { withNamespaces } from 'react-i18next';

import { withStyles } from '@material-ui/core/styles';

import Typography from '@material-ui/core/Typography';

import SimpleLineChart from '../../components/Home/SimpleLineChart';
import SimpleTable from '../../components/Home/SimpleTable';

const HomePage = ({ classes }) => (
  <Page>
    <>
      <Typography variant="h4" gutterBottom component="h2">
        Orders
      </Typography>
      <Typography component="div" className={classes.chartContainer}>
        <SimpleLineChart />
      </Typography>
      <Typography variant="h4" gutterBottom component="h2">
        Products
      </Typography>
      <div className={classes.tableContainer}>
        <SimpleTable />
      </div>
    </>
  </Page>
);

export default withNamespaces()(withStyles(styles)(HomePage));
