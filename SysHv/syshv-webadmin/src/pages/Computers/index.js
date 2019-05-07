import React from 'react';

import Page from '../page';
import Button from '@material-ui/core/Button';
import ExpansionPanel from '@material-ui/core/ExpansionPanel';
import ExpansionPanelDetails from '@material-ui/core/ExpansionPanelDetails';
import ExpansionPanelSummary from '@material-ui/core/ExpansionPanelSummary';
import Typography from '@material-ui/core/Typography';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';

import { getClients } from '../../actions/client';
import { addSensor } from '../../actions/sensor';
import { connectTo } from '../../utils';

const clientStyles = theme => ({
  root: {
    width: '100%'
  },
  heading: {
    fontSize: theme.typography.pxToRem(15),
    flexBasis: '33.33%',
    flexShrink: 0
  },
  secondaryHeading: {
    fontSize: theme.typography.pxToRem(15),
    color: theme.palette.text.secondary
  }
});

const Client = withStyles(clientStyles)(({ classes, client }) => (
  <ExpansionPanel>
    <ExpansionPanelSummary expandIcon={<ExpandMoreIcon />}>
      <Typography className={classes.heading}>{client.name}</Typography>
      <Typography className={classes.secondaryHeading}>
        {client.description}
      </Typography>
    </ExpansionPanelSummary>
    <ExpansionPanelDetails>
      <div>
        {Object.keys(client).map(k => (
          <Typography key={k}>{`${k} : ${client[k]}`}</Typography>
        ))}
      </div>
    </ExpansionPanelDetails>
  </ExpansionPanel>
));

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
          <Client key={c.id} client={c} />
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
