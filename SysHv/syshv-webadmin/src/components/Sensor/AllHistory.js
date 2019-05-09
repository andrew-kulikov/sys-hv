import React from 'react';

import Chart from 'react-apexcharts';

import { connectTo } from '../../utils';
import moment from 'moment';

class SimpleLineChart extends React.Component {
  state = {
    options: {
      chart: {
        id: this.props.name,
        toolbar: {
          show: true
        },
        zoom: {
          enabled: true
        }
      },
      dataLabels: {
        enabled: false
      },
      stroke: {
        curve: 'smooth'
      },
      title: {
        text: this.props.title,
        align: 'left'
      },
      xaxis: {
        type: 'datetime',
        tickAmount: 4,
        labels: {
          formatter: function(val, timestamp) {
            return moment(timestamp).format('DD/MM/YYYY');
          }
        }
      }
    },
    series: [
      {
        data: this.props.data.map(l => ({
          x: l.time,
          y: JSON.parse(l.logJson).Value
        }))
      }
    ]
  };

  render() {
    console.log(this.props.data);
    return (
      <Chart
        options={this.state.options}
        series={this.state.series}
        type="area"
        height={this.props.height}
      />
    );
  }
}

export default connectTo(
  state => ({ data: state.history.history }),
  {},
  SimpleLineChart
);
