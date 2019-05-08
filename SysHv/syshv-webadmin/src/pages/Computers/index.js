import React from 'react';

import Page from '../page';
import Button from '@material-ui/core/Button';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';

import Link from '@material-ui/core/Link';
import { Link as RouterLink } from 'react-router-dom';

import CheckIcon from '@material-ui/icons/Check';
import AlertIcon from '@material-ui/icons/AddAlertOutlined';

import { styles, clientStyles } from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';

import { getClients } from '../../actions/client';
import { addSensor } from '../../actions/sensor';
import { connectTo } from '../../utils';

import moment from 'moment';

const Client = withStyles(clientStyles)(({ classes, client, updates }) => (
  <ExpansionPanel>
    <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
      <Typography className={classes.heading}>{`Client #${client.id} - ${
        client.name
      }. IP: ${client.ip}`}</Typography>
    </ExpansionPanelSummary>
    <ExpansionPanelDetails>
      <Table className={classes.table}>
        <TableHead>
          <TableRow>
            <TableCell>Sensor</TableCell>
            <TableCell align="center">Last Update</TableCell>
            <TableCell align="center">Last Value</TableCell>
            <TableCell align="center">Status</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {client.clientSensors.map(sensor => {
            const sensorUpdates = updates[sensor.id];
            let lastUpdateDate = '-',
              lastValue = '-',
              lastStatus = '-';
            if (sensorUpdates && sensorUpdates.length > 0) {
              const lastUpdate = sensorUpdates[sensorUpdates.length - 1];
              lastUpdateDate = moment(new Date(lastUpdate.Time)).format(
                'HH:mm:ss'
              );
              lastValue = lastUpdate.Value.Value;
              lastStatus =
                lastUpdate.Value.Status == 'OK' ? <CheckIcon /> : <AlertIcon />;
            }
            return (
              <TableRow key={sensor.id}>
                <TableCell component="th" scope="row">
                  <Link component={RouterLink} to={'/sensor/' + sensor.id}>
                    {sensor.name}
                  </Link>
                </TableCell>
                <TableCell align="center" component="th" scope="row">
                  {lastUpdateDate}
                </TableCell>
                <TableCell align="center" component="th" scope="row">
                  {lastValue}
                  {sensor.ValueUnit}
                </TableCell>
                <TableCell align="center" component="th" scope="row">
                  {lastStatus}
                </TableCell>
              </TableRow>
            );
          })}
        </TableBody>
      </Table>
    </ExpansionPanelDetails>
  </ExpansionPanel>
));

class ComputersPage extends React.Component {
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
          <Button onClick={() => this.props.addSensor({ ClientId: 1 })}>
            Add
          </Button>
        </div>
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
  { getClients, addSensor },
  withNamespaces()(withStyles(styles)(ComputersPage))
);
