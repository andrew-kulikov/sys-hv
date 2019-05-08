import React from 'react';

import Page from '../page';
import ReactApexChart from 'react-apexcharts';

import Paper from '@material-ui/core/Paper';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';
import { connectTo } from '../../utils';

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
              offset: 20,
              color: '#61DBC3',
              opacity: 1
            },
            {
              offset: 60,
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
    },
    series: [100]
  };

  componentDidMount() {}

  render() {
    const { classes, match } = this.props;
    return (
      <Page title={`Sensor - ${match.params.id}`}>
        <Paper>
          <ReactApexChart
            options={this.state.options}
            series={this.state.series}
            type="radialBar"
            height="450"
          />
        </Paper>
      </Page>
    );
  }
}

export default withNamespaces()(
  withStyles(styles)(connectTo(state => ({}), {}, SensorPage))
);
