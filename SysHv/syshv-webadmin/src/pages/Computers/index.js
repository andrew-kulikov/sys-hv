import React from 'react';

import Page from '../page';
import Client from '../../components/client/Client';
import AddClientModal from '../../components/client/AddClientModal';

import Button from '@material-ui/core/Button';

import { styles } from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';

import { getClients, addClient } from '../../actions/client';
import { addSensor } from '../../actions/sensor';
import { connectTo } from '../../utils';

class ComputersPage extends React.Component {
  state = {
    modalOpen: false
  };

  handleModalOpen = () => {
    this.setState({ modalOpen: true });
  };

  handleModalClose = () => {
    this.setState({ modalOpen: false });
  };

  handleSubmitClient = client => {
    this.props.addClient(client);
    this.setState({ modalOpen: false });
  };

  componentDidMount() {
    this.props.getClients();
  }

  render() {
    const { clients, updates } = this.props;

    return (
      <Page title="Clients">
        {clients.map(c => (
          <Client key={c.id} client={c} updates={updates.sensorValues} />
        ))}
        <div>
          <Button onClick={this.handleModalOpen}>Add</Button>
        </div>
        <AddClientModal
          open={this.state.modalOpen}
          handleClose={this.handleModalClose}
          handleSubmit={this.handleSubmitClient}
        />
      </Page>
    );
  }
}

export default connectTo(
  state => ({
    clients: state.client.clients,
    updates: state.sensor,
    allSensors: state.allSensors
  }),
  { getClients, addSensor, addClient },
  withNamespaces()(withStyles(styles)(ComputersPage))
);
