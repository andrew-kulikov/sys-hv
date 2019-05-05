import React from 'react';

import ApexCtarts from 'apexcharts';
import Chart from 'react-apexcharts';

import { connectTo } from '../../utils';

var lastDate = 0;
var data = [];

function getDayWiseTimeSeries(baseval, count, yrange) {
  var i = 0;
  while (i < count) {
    var x = baseval;
    var y =
      Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

    data.push({
      x,
      y
    });
    lastDate = baseval;
    baseval += 86400000;
    i++;
  }
}

getDayWiseTimeSeries(new Date('11 Feb 2017 GMT').getTime(), 10, {
  min: 10,
  max: 90
});

function getNewSeries(baseval, yrange) {
  var newDate = baseval + 86400000;
  lastDate = newDate;
  data.push({
    x: newDate,
    y: Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min
  });
}

function resetData() {
  data = data.slice(data.length - 10, data.length);
}

class SimpleLineChart extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
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
          type: 'datetime',
          range: 777600000
        },
        yaxis: {
          max: 100
        },
        legend: {
          show: false
        }
      },
      series: [
        {
          data: data.slice()
        }
      ]
    };
  }

  componentDidMount() {
    this.intervals();
  }

  intervals() {
    /*
    window.setInterval(() => {
      getNewSeries(lastDate, {
        min: 10,
        max: 90
      });

      this.setState({
        ...this.state,
        series: [
          {
            data: data.slice()
          }
        ]
      });

      //ApexCtarts.exec('realtime', 'updateSeries', [{ data: data }]);
    }, 5000);*/

    // every 60 seconds, we reset the data
    window.setInterval(() => {
      resetData();

      ApexCtarts.exec(
        'realtime',
        'updateSeries',
        [{ data: data }],
        false,
        true
      );
    }, 60000);
  }
  render() {
    console.log(data);
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

export default connectTo(state => ({ data: state.sensor.sensorValues }), {}, SimpleLineChart);
