import React from 'react';

import { connectTo } from '../../utils';
import styles from './style';
import Page from '../page';
import { withNamespaces } from 'react-i18next';

import { withStyles } from '@material-ui/core/styles';

class SensorsPage extends React.Component {
  render() {
    const { classes } = this.props;

    return (
      <Page>
        <div>Sensors</div>
      </Page>
    );
  }
}

export default connectTo(
  state => ({
    token: state.auth.token
  }),
  {},
  withNamespaces()(withStyles(styles)(SensorsPage))
);
