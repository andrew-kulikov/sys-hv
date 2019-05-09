import React from 'react';

import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';

import ReactJson from 'react-json-view'

import styles from './style';
import Page from '../page';
import { withNamespaces } from 'react-i18next';

import { withStyles } from '@material-ui/core/styles';

import { getLastHistory } from '../../actions/history';
import { connectTo } from '../../utils';

class LogsPage extends React.Component {
  componentDidMount() {
    if (!this.props.history || !this.props.history.history.length)
      this.props.getLastHistory();
  }
  render() {
    const { history } = this.props;

    return (
      <Page title="Logs">
        <div>Logs</div>
        <List>
          {history.history.slice(0, 100).map(log => (
            <ListItem key={log.id}><ReactJson src={JSON.parse(log.logJson)} /></ListItem>
          ))}
        </List>
      </Page>
    );
  }
}

export default connectTo(
  state => ({ history: state.history }),
  { getLastHistory },
  withStyles(styles)(LogsPage)
);
