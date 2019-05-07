import React from 'react';

import Page from '../page';
import Button from '@material-ui/core/Button';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';

import { getClients } from '../../actions/client';
import { addSensor } from '../../actions/sensor';
import { connectTo } from '../../utils';

class ComputersPage extends React.Component {
  componentDidMount() {
    this.props.getClients();
  }
  render() {
    const { clients } = this.props;
    console.log(clients);
    return (
      <Page>
        <div>Clients:</div>
        {clients.map(c => (
          <div key={c.id}>
            {Object.keys(c).map(k => (
              <div key={k}>{`${k} : ${c[k]}`}</div>
            ))}
          </div>
        ))}
        <div>
          <Button onClick={() => this.props.addSensor({ ClientId: 1 })}>
            Add
          </Button>
        </div>
      </Page>
    );
  }
}

export default connectTo(
  state => ({ clients: state.client.clients }),
  { getClients, addSensor },
  withNamespaces()(withStyles(styles)(ComputersPage))
);
