import React from 'react';

import ReactApexChart from 'react-apexcharts';

import Paper from '@material-ui/core/Paper';
import Typography from '@material-ui/core/Typography';

import styles from './style';
import { withNamespaces } from 'react-i18next';
import { withStyles } from '@material-ui/core/styles';
import { connectTo } from '../../utils';

import { getClientSensor } from '../../actions/sensor';

class RadialGraphs extends React.Component {

  getChartOptions = ({ minValue, maxValue, criticalValue, name }) => ({
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
            offset: minValue,
            color: '#95DA74',
            opacity: 1
          },
          {
            offset: criticalValue,
            color: '#FAD375',
            opacity: 1
          },
          {
            offset: maxValue,
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
    labels: [name]
  });

  render() {
    const { classes, selectedSensor } = this.props;

    const values = selectedSensor.values;
    let currentValue = 0;
    if (values.length) currentValue = values[values.length - 1].y;

    return (
      <div style={{ display: 'flex', flexWrap: 'wrap' }}>
        <Paper className={classes.sensorCard}>
          <Typography component="h5" variant="h4">
            Sensor status - Total
          </Typography>
          <ReactApexChart
            options={this.getChartOptions(selectedSensor.sensor)}
            series={[currentValue]}
            type="radialBar"
            height="450"
          />
        </Paper>
        {Object.keys(selectedSensor.subsensors).map(k => {
          const values = selectedSensor.subsensors[k];
          let lastTick = 0;
          if (values.length) lastTick = values[values.length - 1].y;
          return (
            <Paper className={classes.smallSensor} key={k}>
              <ReactApexChart
                options={this.getChartOptions({
                  ...selectedSensor.sensor,
                  name: k
                })}
                series={[lastTick]}
                type="radialBar"
                height="250"
              />
            </Paper>
          );
        })}
        <Paper className={classes.info}>
          <div>Last update:</div>
          <div>Last status:</div>
        </Paper>
      </div>
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
      RadialGraphs
    )
  )
);
