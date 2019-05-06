import React from 'react';

import Page from '../page';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';

const ComputersPage = ({ classes }) => (
  <Page>
    <div>Computers</div>
  </Page>
);

export default withNamespaces()(withStyles(styles)(ComputersPage));
