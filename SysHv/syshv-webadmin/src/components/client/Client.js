import React from 'react';

import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import IconButton from '@material-ui/core/IconButton';

import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';

import Link from '@material-ui/core/Link';
import { Link as RouterLink } from 'react-router-dom';

import CheckIcon from '@material-ui/icons/Check';
import DeleteIcon from '@material-ui/icons/Delete';
import AlertIcon from '@material-ui/icons/AddAlertOutlined';

import { clientStyles } from './style';
import { withStyles } from '@material-ui/core/styles';

import moment from 'moment';

const Client = withStyles(clientStyles)(
  ({ classes, client, updates, deleteClient }) => (
    <ExpansionPanel>
      <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
        <Typography className={classes.heading}>{`Client #${client.id} - ${
          client.name
        }. IP: ${client.ip}`}</Typography>
      </ExpansionPanelSummary>
      <ExpansionPanelDetails
        style={{ display: 'flex', flexDirection: 'column' }}
      >
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
                  lastUpdate.Value.Status == 'OK' ? (
                    <CheckIcon />
                  ) : (
                    <AlertIcon />
                  );
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

        <IconButton
          aria-label="Delete"
          className={classes.deleteButton}
          onClick={() => deleteClient(client.id)}
        >
          <DeleteIcon fontSize="default" />
        </IconButton>
      </ExpansionPanelDetails>
    </ExpansionPanel>
  )
);

export default Client;
