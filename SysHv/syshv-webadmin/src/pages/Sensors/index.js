import React from 'react';

import Page from '../page';

import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Button from '@material-ui/core/Button';
import Typography from '@material-ui/core/Typography';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';

import { connectTo } from '../../utils';
import styles from './style';

import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';

import { getSensors } from '../../actions/sensor';
import { getOsType } from '../../utils/tools';

class SensorsPage extends React.Component {
  componentDidMount() {
    this.props.getSensors();
  }

  render() {
    const { classes } = this.props;

    return (
      <Page title="All Sensors">
        <Typography variant="h4" component="h2" style={{ marginLeft: '10px' }}>
          All sensors available in system:
        </Typography>
        <div style={{ display: 'flex' }}>
          {this.props.sensors.map(s => (
            <Card className={classes.card} key={s.id}>
              <CardContent>
                <Typography
                  className={classes.title}
                  color="textSecondary"
                  gutterBottom
                >
                  Sensor #{s.id}
                </Typography>
                <Typography variant="h5" component="h2">
                  {s.name}
                </Typography>
                <Typography className={classes.pos} color="textSecondary">
                  OS: {getOsType(s.osType)}; Units: {s.valueUnit}
                </Typography>
                <Typography component="p">{s.description}</Typography>
                <div style={{ marginTop: '10px' }}>
                  <Typography variant="h6" component="h2">
                    Subsensors:
                  </Typography>
                  <List>
                    {s.subSensors.map(subsensor => (
                      <ListItem
                        key={subsensor.id}
                        style={{ padding: '0' }}
                        button
                      >
                        <ListItemText>
                          <Typography>{`${subsensor.name} - ${
                            subsensor.description
                          }`}</Typography>
                        </ListItemText>
                      </ListItem>
                    ))}
                  </List>
                </div>
              </CardContent>
              <CardActions>
                <Button size="small">Learn More</Button>
              </CardActions>
            </Card>
          ))}
        </div>
      </Page>
    );
  }
}

export default connectTo(
  state => ({
    sensors: state.allSensors.sensors
  }),
  { getSensors },
  withNamespaces()(withStyles(styles)(SensorsPage))
);
