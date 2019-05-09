import React from 'react';

import Page from '../page';
import Client from '../../components/client/Client';
import AddClientModal from '../../components/client/AddClientModal';
import AddSensorModal from '../../components/Sensor/AddSensorModal';

import Tooltip from '@material-ui/core/Tooltip';
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
    addClientModalOpen: false,
    addSensorModalOpen: false,
    addSensorModalClient: {}
  };

  handleAddClientModalOpen = () => {
    this.setState({ addClientModalOpen: true });
  };

  handleAddClientModalClose = () => {
    this.setState({ addClientModalOpen: false });
  };

  handleSubmitClient = client => {
    this.props.addClient(client);
    this.handleAddClientModalClose();
  };

  handleAddSensorModalOpen = client => {
    this.setState({ addSensorModalOpen: true, addSensorModalClient: client });
  };

  handleAddSensorModalClose = () => {
    this.setState({ addSensorModalOpen: false });
  };

  handleSubmitSensor = sensor => {
    this.props.addSensor(sensor);
   
    this.handleAddSensorModalClose();
  };

  componentDidMount() {
    this.props.getClients();
  }

  render() {
    const { classes, clients, updates } = this.props;

    return (
      <Page title="Clients">
        {clients.map(c => (
          <Client
            key={c.id}
            client={c}
            updates={updates.sensorValues}
            deleteClient={this.props.deleteClient}
            handleOpenAddSensor={this.handleAddSensorModalOpen}
          />
        ))}
        <div className={classes.buttonContainer}>
          <IconButton
            aria-label="Add Client"
            className={classes.addIcon}
            onClick={this.handleAddClientModalOpen}
          >
            <Tooltip title="Add Client" aria-label="Add Client">
              <AddIcon fontSize="large" />
            </Tooltip>
          </IconButton>
        </div>
        <AddClientModal
          open={this.state.addClientModalOpen}
          handleClose={this.handleAddClientModalClose}
          handleSubmit={this.handleSubmitClient}
        />
        <AddSensorModal
          open={this.state.addSensorModalOpen}
          handleClose={this.handleAddSensorModalClose}
          handleSubmit={this.handleSubmitSensor}
          client={this.state.addSensorModalClient}
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
