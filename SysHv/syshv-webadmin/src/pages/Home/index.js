import React from 'react';

import { connectTo } from '../../utils';
import styles from './style';
import Page from '../page';
import { withNamespaces } from 'react-i18next';

import { withStyles } from '@material-ui/core/styles';

import Typography from '@material-ui/core/Typography';

import SimpleLineChart from '../../components/Home/SimpleLineChart';
import SimpleTable from '../../components/Home/SimpleTable';

class HomePage extends React.Component {
  componentDidMount() {
    !this.props.token && this.props.history.replace('/login');
  }

  componentDidUpdate() {
    !this.props.token && this.props.history.replace('/login');
  }

  render() {
    const { classes } = this.props;

    return (
      <Page>
        <main className={classes.content}>
          <div className={classes.appBarSpacer} />
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
        </main>
      </Page>
    );
  }
}

export default connectTo(
  state => ({
    token: state.auth.token
  }),
  {},
  withNamespaces()(withStyles(styles)(HomePage))
);
