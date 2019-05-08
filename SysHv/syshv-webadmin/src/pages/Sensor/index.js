import React from 'react';

import Page from '../page';
import ReactApexChart from 'react-apexcharts';

import Paper from '@material-ui/core/Paper';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';
import { connectTo } from '../../utils';

import { getClientSensor } from '../../actions/sensor';

class SensorPage extends React.Component {
  state = {
    options: {
      chart: {
        toolbar: {
          show: true
        }
      },
      plotOptions: {
        radialBar: {
          startAngle: -135,
          endAngle: 135,
          hollow: {
            margin: 0,
            size: '70%',
            background: '#fff',
            position: 'front',
            dropShadow: {
              enabled: true,
              top: 3,
              left: 0,
              blur: 4,
              opacity: 0.24
            }
          },
          track: {
            background: '#fff',
            strokeWidth: '67%',
            margin: 0, // margin is in pixels
            dropShadow: {
              enabled: true,
              top: -3,
              left: 0,
              blur: 4,
              opacity: 0.35
            }
          },

          dataLabels: {
            name: {
              offsetY: -10,
              show: true,
              color: '#888',
              fontSize: '17px'
            },
            value: {
              formatter: function(val) {
                return parseInt(val);
              },
              color: '#111',
              fontSize: '36px',
              show: true
            }
          }
        }
      },
      fill: {
        type: 'gradient',
        gradient: {
          shade: 'dark',
          type: 'horizontal',
          shadeIntensity: 0.5,
          colorStops: [
            {
              offset: 0,
              color: '#95DA74',
              opacity: 1
            },
            {
              offset: 90,
              color: '#FAD375',
              opacity: 1
            },
            {
              offset: 100,
              color: '#EB656F',
              opacity: 1
            }
          ],

          opacityFrom: 1,
          opacityTo: 1
        }
      },
      stroke: {
        lineCap: 'round'
      },
      labels: ['Percent']
    }
  };

  componentDidMount() {
    this.props.getClientSensor(this.props.match.params.id);
  }

  static getDerivedStateFromProps(nextProps, prevState) {
    return {
      options: {
        ...prevState.options,
        fill: {
          type: 'gradient',
          gradient: {
            shade: 'dark',
            type: 'horizontal',
            shadeIntensity: 0.5,
            colorStops: [
              {
                offset: nextProps.selectedSensor.sensor.minValue,
                color: '#95DA74',
                opacity: 1
              },
              {
                offset: nextProps.selectedSensor.sensor.criticalValue,
                color: '#FAD375',
                opacity: 1
              },
              {
                offset: nextProps.selectedSensor.sensor.maxValue,
                color: '#EB656F',
                opacity: 1
              }
            ],

            opacityFrom: 1,
            opacityTo: 1
          }
        }
      }
    };
  }

  render() {
    const { classes, match, selectedSensor } = this.props;
    console.log(selectedSensor);
    const values = selectedSensor.values;
    let currentValue = 0;
    if (values.length) currentValue = values[values.length - 1].y;

    return (
      <Page title={`Sensor - ${match.params.id}`}>
        <Paper>
          <ReactApexChart
            options={this.state.options}
            series={[currentValue]}
            type="radialBar"
            height="450"
          />
        </Paper>
      </Page>
    );
  }
}

export default withNamespaces()(
  withStyles(styles)(
    connectTo(
      state => ({
        selectedSensor: state.selectedSensor
      }),
      {
        getClientSensor
      },
      SensorPage
    )
  )
);
