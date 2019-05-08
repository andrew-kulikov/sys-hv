import React from 'react';

import Page from '../page';

import Typography from '@material-ui/core/Typography';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';

import RadialGraphs from '../../components/Sensor/RadialGraphs';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';
import { connectTo } from '../../utils';

import { getClientSensor } from '../../actions/sensor';

function TabContainer(props) {
  return (
    <Typography component="div" style={{ padding: 8 * 3 }}>
      {props.children}
    </Typography>
  );
}

class SensorPage extends React.Component {
  state = {
    value: 0
  };

  componentDidMount() {
    this.props.getClientSensor(this.props.match.params.id);
  }

  handleChange = (event, value) => {
    this.setState({ value });
  };

  render() {
    const { classes, match } = this.props;
    const { value } = this.state;

    return (
      <Page title={`Sensor - ${match.params.id}`}>
        <div className={classes.root}>
          <div>
            <Tabs value={value} onChange={this.handleChange}>
              <Tab label="Sensors" />
              <Tab label="Live graph" />
              <Tab label="Last 2 days" />
              <Tab label="Last month" />
              <Tab label="All history" />
              <Tab label="Logs" />
            </Tabs>
          </div>
          {value === 0 && (
            <TabContainer>
              <RadialGraphs id={match.params.id} />
            </TabContainer>
          )}
          {value === 1 && <TabContainer>Live graph</TabContainer>}
          {value === 2 && <TabContainer>Last 2 days</TabContainer>}
          {value === 3 && <TabContainer>Last month</TabContainer>}
          {value === 4 && <TabContainer>All history</TabContainer>}
          {value === 5 && <TabContainer>Logs</TabContainer>}
        </div>
      </Page>
    );
  }
}

export default withNamespaces()(
  withStyles(styles)(
    connectTo(
      state => ({
      }),
      {
        getClientSensor
      },
      SensorPage
    )
  )
);
