import React from 'react';

import { withStyles } from '@material-ui/core/styles';
import { withRouter } from 'react-router-dom';

import Footer from '../containers/Footer';
import Header from '../containers/Header';

import Divider from '@material-ui/core/Divider';
import IconButton from '@material-ui/core/IconButton';
import Badge from '@material-ui/core/Badge';
import MenuIcon from '@material-ui/icons/Menu';
import ChevronLeftIcon from '@material-ui/icons/ChevronLeft';
import NotificationsIcon from '@material-ui/icons/Notifications';
import {
  MainListItems,
  SecondaryListItems
} from '../components/Home/ListItems';
import CssBaseline from '@material-ui/core/CssBaseline';
import Drawer from '@material-ui/core/Drawer';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import List from '@material-ui/core/List';
import Typography from '@material-ui/core/Typography';

import classNames from 'classnames';

import styles from './style';

import { connectTo } from '../utils';

class Page extends React.Component {
  state = {
    open: true
  };

  componentDidMount() {
    !this.props.token && this.props.history.replace('/login');
  }

  componentDidUpdate() {
    !this.props.token && this.props.history.replace('/login');
  }

  handleDrawerOpen = () => {
    this.setState({ open: true });
  };

  handleDrawerClose = () => {
    this.setState({ open: false });
  };
  render() {
    const { children, classes } = this.props;

    return (
      <div className={classes.root}>
        <CssBaseline />
        <AppBar
          position="absolute"
          className={classNames(
            classes.appBar,
            this.state.open && classes.appBarShift
          )}
        >
          <Toolbar
            disableGutters={!this.state.open}
            className={classes.toolbar}
          >
            <IconButton
              color="inherit"
              aria-label="Open drawer"
              onClick={this.handleDrawerOpen}
              className={classNames(
                classes.menuButton,
                this.state.open && classes.menuButtonHidden
              )}
            >
              <MenuIcon />
            </IconButton>
            <Typography
              component="h1"
              variant="h6"
              color="inherit"
              noWrap
              className={classes.title}
            >
              Dashboard
            </Typography>
            <IconButton color="inherit">
              <Badge badgeContent={4} color="secondary">
                <NotificationsIcon />
              </Badge>
            </IconButton>
          </Toolbar>
        </AppBar>
        <Drawer
          variant="permanent"
          classes={{
            paper: classNames(
              classes.drawerPaper,
              !this.state.open && classes.drawerPaperClose
            )
          }}
          open={this.state.open}
        >
          <div className={classes.toolbarIcon}>
            <IconButton onClick={this.handleDrawerClose}>
              <ChevronLeftIcon />
            </IconButton>
          </div>
          <Divider />
          <List>
            <MainListItems />
          </List>
          <Divider />
          <List>
            <SecondaryListItems />
          </List>
        </Drawer>

        <main className={classes.content}>
          <div className={classes.appBarSpacer} />
          {children}
        </main>
        <Footer />
      </div>
    );
  }
}

export default connectTo(
  state => ({
    token: state.auth.token
  }),
  { },
  withRouter(withStyles(styles)(Page))
);
