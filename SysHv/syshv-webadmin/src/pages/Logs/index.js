import React from 'react';

import styles from './style';
import Page from '../page';
import { withNamespaces } from 'react-i18next';

import { withStyles } from '@material-ui/core/styles';

const LogsPage = ({ classes }) => (
  <Page title="Logs">
    <div>Logs</div>
  </Page>
);

export default withNamespaces()(withStyles(styles)(LogsPage));
