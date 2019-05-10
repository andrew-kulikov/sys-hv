import React from 'react';

import Chart from 'react-apexcharts';

import { connectTo } from '../../utils';
import moment from 'moment';

class SimpleLineChart extends React.Component {
  state = {
    options: {
      chart: {
        id: this.props.name,
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
        text: this.props.title,
        align: 'left'
      },
      xaxis: {
        type: 'datetime',
        min: Date.now(),
        range: 50000,
        tickAmount: 4,
        labels: {
          formatter: function(val, timestamp) {
            return moment(timestamp).format('HH:mm:ss');
          }
        }
      }
    },
    series: [{ data: [] }]
  };

  static getDerivedStateFromProps(nextProps, prevState) {
    return {
      ...prevState,
      series: [
        {
          data: nextProps.data.slice()
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
        height={this.props.height}
      />
    );
  }
}

const Charts = props => {
  const { data } = props;
  const subsensors = data.subsensors;

  return (
    <>
      <SimpleLineChart
        name="main"
        title={'Total'}
        height="350"
        data={data.values}
      />
      <div style={{ display: 'flex' }}>
        {Object.keys(subsensors).map(name => (
          <div key={name} style={{ width: '48%' }}>
            <SimpleLineChart
              name="name"
              title={name}
              height="320"
              data={subsensors[name]}
            />
          </div>
        ))}
      </div>
    </>
  );
};

export default connectTo(state => ({ data: state.selectedSensor }), {}, Charts);
