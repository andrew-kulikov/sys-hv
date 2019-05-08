import React from 'react';

import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';

class AddClientDialog extends React.Component {
  state = {
    ip: '',
    name: '',
    description: ''
  };

  handleChange = event => {
    this.setState({
      [event.target.name]: event.target.value
    });
  };

  render() {
    const { open, handleClose, handleSubmit } = this.props;

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
            <TextField
              autoFocus
              margin="dense"
              id="ip"
              name="ip"
              value={this.state.ip}
              label="Ip"
              type="ip"
              fullWidth
              onChange={this.handleChange}
            />
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
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose} color="primary">
              Cancel
            </Button>
            <Button onClick={() => handleSubmit(this.state)} color="primary">
              Register
            </Button>
          </DialogActions>
        </Dialog>
      </div>
    );
  }
}

export default AddClientDialog;
