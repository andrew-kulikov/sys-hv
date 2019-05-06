import React from 'react';

import { connectTo } from '../../utils';
import styles from './style';
import Page from '../page';
import { withNamespaces } from 'react-i18next';

import { withStyles } from '@material-ui/core/styles';

const LogsPage = ({ classes }) => (
  <Page>
    <div>Logs</div>
  </Page>
);

export default withNamespaces()(withStyles(styles)(LogsPage));
