import React from 'react';

import { connectTo } from '../../utils';
import styles from './style';
import Page from '../page';
import { withNamespaces } from 'react-i18next';

import { withStyles } from '@material-ui/core/styles';

class LogsPage extends React.Component {
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
        <div>Logs</div>
      </Page>
    );
  }
}

export default connectTo(
  state => ({
    token: state.auth.token
  }),
  {},
  withNamespaces()(withStyles(styles)(LogsPage))
);
