import React from 'react';

import ApexCtarts from 'apexcharts';
import Chart from 'react-apexcharts';

import { connectTo } from '../../utils';
import moment from 'moment';


class SimpleLineChart extends React.Component {
  state = {
    options: {
      chart: {
        id: 'realtime',
        animations: {
          enabled: true,
          easing: 'linear',
          dynamicAnimation: {
            speed: 5000
          }
        },
        toolbar: {
          show: false
        },
        zoom: {
          enabled: false
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: 'smooth'
      },

      title: {
        text: 'Dynamic Updating Chart',
        align: 'left'
      },
      markers: {
        size: 0
      },
      xaxis: {
        x: new Date('14 Nov 2012').getTime(),
        type: 'datetime',
        min: Date.now(),
        range: 50000,
        tickAmount: 2,
        labels: {
          formatter: function(val, timestamp) {
            return moment(timestamp).format('HH:mm:ss');
          }
        }
      },
      yaxis: {
        min: 0,
        max: 100
      },
      legend: {
        show: false
      }
    },
    series: [
      {
        data: []
      }
    ]
  };

  static getDerivedStateFromProps(nextProps, prevState) {
    return {
      ...prevState,
      series: [
        {
          data: nextProps.data.values.slice()
        }
      ]
    };
  }

  render() {
    return (
      <Chart
        options={this.state.options}
        series={this.state.series}
        type="line"
        height="350"
      />
    );
  }
}

export default connectTo(
  state => ({ data: state.selectedSensor }),
  {},
  SimpleLineChart
);
