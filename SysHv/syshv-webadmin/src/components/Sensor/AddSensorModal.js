import React from 'react';

import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import Select from '@material-ui/core/Select';
import FormControl from '@material-ui/core/FormControl';

import { connectTo } from '../../utils';
import { getSensors } from '../../actions/sensor';
import { withStyles } from '@material-ui/core/styles';

import styles from './style';

class AddClientDialog extends React.Component {
  state = {
    name: '',
    description: '',
    interval: '',
    criticalValue: '',
    warningValue: '',
    sensorId: ''
  };

  componentDidMount() {
    this.props.getSensors();
  }

  handleChange = event => {
    this.setState({
      [event.target.name]: event.target.value
    });
  };

  render() {
    const { classes, open, handleClose, handleSubmit, client } = this.props;

    return (
      <div>
        <Dialog
          open={open}
          onClose={handleClose}
          aria-labelledby="form-dialog-title"
        >
          <DialogTitle id="form-dialog-title">Add Client</DialogTitle>
          <DialogContent>
            <DialogContentText>
              To add new client, please enter it's ip, name and description.
              Then configure service on client machine and enjoy!
            </DialogContentText>
            <FormControl className={classes.sensorSelect}>
              <InputLabel htmlFor="sensorId">Sensor</InputLabel>
              <Select
                value={this.state.sensorId}
                onChange={this.handleChange}
                inputProps={{
                  name: 'sensorId',
                  id: 'sensorId'
                }}
              >
                <MenuItem value="">
                  <em>None</em>
                </MenuItem>
                {this.props.allSensors.sensors.map(s => (
                  <MenuItem key={s.id} value={s.id}>
                    {s.name}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <TextField
              margin="dense"
              id="name"
              name="name"
              value={this.state.name}
              label="Name"
              fullWidth
              onChange={this.handleChange}
            />
            <TextField
              margin="dense"
              id="description"
              name="description"
              value={this.state.description}
              label="Description"
              fullWidth
              onChange={this.handleChange}
            />
            <TextField
              margin="dense"
              id="interval"
              name="interval"
              value={this.state.interval}
              label="Interval"
              fullWidth
              onChange={this.handleChange}
            />
            <TextField
              margin="dense"
              id="warningValue"
              name="warningValue"
              value={this.state.warningValue}
              label="Warning Value"
              fullWidth
              onChange={this.handleChange}
            />
            <TextField
              margin="dense"
              id="criticalValue"
              name="criticalValue"
              value={this.state.criticalValue}
              label="Critical Value"
              fullWidth
              onChange={this.handleChange}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose} color="primary">
              Cancel
            </Button>
            <Button
              onClick={() =>
                handleSubmit({ ...this.state, clientId: client.id })
              }
              color="primary"
            >
              Register
            </Button>
          </DialogActions>
        </Dialog>
      </div>
    );
  }
}

export default connectTo(
  state => ({ allSensors: state.allSensors }),
  {
    getSensors
  },
  withStyles(styles)(AddClientDialog)
);
