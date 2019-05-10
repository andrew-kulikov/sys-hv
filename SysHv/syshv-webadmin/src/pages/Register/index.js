import React from 'react';

import Paper from '@material-ui/core/Paper';
import Input from '@material-ui/core/Input';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';
import InputLabel from '@material-ui/core/InputLabel';
import CssBaseline from '@material-ui/core/CssBaseline';
import FormControl from '@material-ui/core/FormControl';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';

import styles from './style';
import { connectTo } from '../../utils';
import { withNamespaces } from 'react-i18next';
import withStyles from '@material-ui/core/styles/withStyles';

import { register } from '../../actions/auth';

class RegisterPage extends React.Component {
  state = {
    email: '',
    password: '',
    passwordRepeat: ''
  };

  handleChange = event => {
    this.setState({
      [event.target.name]: event.target.value
    });
  };

  handleSubmit = e => {
    this.props.register({
      email: this.state.email,
      password: this.state.password
    });
    e.preventDefault();
  };

  componentDidMount() {
    this.props.token && this.props.history.replace('/');
  }

  componentDidUpdate() {
    this.props.token && this.props.history.replace('/');
  }

  render() {
    const { classes, t } = this.props;

    return (
      <main className={classes.main}>
        <CssBaseline />
        <Paper className={classes.paper}>
          <Avatar className={classes.avatar}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Sign in
          </Typography>
          <form className={classes.form}>
            <FormControl margin="normal" required fullWidth>
              <InputLabel htmlFor="email">Email Address</InputLabel>
              <Input
                id="email"
                name="email"
                autoComplete="email"
                value={this.state.email}
                onChange={this.handleChange}
                autoFocus
              />
            </FormControl>
            <FormControl margin="normal" required fullWidth>
              <InputLabel htmlFor="password">Password</InputLabel>
              <Input
                value={this.state.password}
                onChange={this.handleChange}
                name="password"
                type="password"
                id="password"
                autoComplete="current-password"
              />
            </FormControl>
            <FormControl margin="normal" required fullWidth>
              <InputLabel htmlFor="passwordRepeat">Repeat Password</InputLabel>
              <Input
                value={this.state.passwordRepeat}
                onChange={this.handleChange}
                name="passwordRepeat"
                type="password"
                id="passwordRepeat"
                autoComplete="current-password"
              />
            </FormControl>
            <Button
              onClick={this.handleSubmit}
              fullWidth
              variant="contained"
              color="primary"
              className={classes.submit}
            >
              Register
            </Button>
          </form>
        </Paper>
      </main>
    );
  }
}

export default connectTo(
  state => ({
    token: state.auth.token
  }),
  {
    register
  },
  withNamespaces()(withStyles(styles)(RegisterPage))
);
