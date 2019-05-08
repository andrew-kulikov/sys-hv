import React from 'react';

import Page from '../page';
import Client from '../../components/client/Client';
import AddClientModal from '../../components/client/AddClientModal';

import IconButton from '@material-ui/core/IconButton';
import AddIcon from '@material-ui/icons/Add';

import { styles } from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';

import { getClients, addClient, deleteClient } from '../../actions/client';
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
    const { classes, clients, updates } = this.props;

    return (
      <Page title="Clients">
        {clients.map(c => (
          <Client key={c.id} client={c} updates={updates.sensorValues} deleteClient={this.props.deleteClient} />
        ))}
        <div className={classes.buttonContainer}>
          <IconButton
            aria-label="Add Client"
            className={classes.addIcon}
            onClick={this.handleModalOpen}
          >
            <AddIcon fontSize="large" />
          </IconButton>
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
  { getClients, addSensor, addClient, deleteClient },
  withNamespaces()(withStyles(styles)(ComputersPage))
);
