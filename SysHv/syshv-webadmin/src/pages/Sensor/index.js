import React from 'react';

import Page from '../page';

import Typography from '@material-ui/core/Typography';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';

import RadialGraphs from '../../components/Sensor/RadialGraphs';
import LiveGraphs from '../../components/Sensor/LiveGraphs';
import AllHistory from '../../components/Sensor/AllHistory';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';
import { connectTo } from '../../utils';

import { getClientSensor } from '../../actions/sensor';
import { getHistory } from '../../actions/history';

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
    this.props.getHistory(this.props.match.params.id);
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
              <Tab label="All history" />
              <Tab label="Logs" />
            </Tabs>
          </div>
          {value === 0 && (
            <TabContainer>
              <RadialGraphs />
            </TabContainer>
          )}
          {value === 1 && (
            <TabContainer>
              <LiveGraphs />
            </TabContainer>
          )}
          {value === 2 && (
            <TabContainer>
              <AllHistory />
            </TabContainer>
          )}
          {value === 3 && <TabContainer>Logs</TabContainer>}
        </div>
      </Page>
    );
  }
}

export default withNamespaces()(
  withStyles(styles)(
    connectTo(
      state => ({ selectedSensor: state.selectedSensor.sensor }),
      {
        getClientSensor,
        getHistory
      },
      SensorPage
    )
  )
);
