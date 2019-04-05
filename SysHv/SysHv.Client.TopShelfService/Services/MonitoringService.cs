using SysHv.Client.TopShelfService.Gatherers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SysHv.Client.TopShelfService.Services
{
    class MonitoringService
    {
        #region Constants

        private const int TimerDelay = 1000;

        #endregion

        #region Fields

        Timer _timer;

        #endregion

        #region Constructors

        public MonitoringService()
        {
            _timer = new Timer(TimerDelay);
            _timer.AutoReset = true;
            _timer.Elapsed += TimerElapsed;
        }

        #endregion


        #region Public Methods

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        #endregion

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            RuntimeInfoGatherer systemInfoGatherer = new RuntimeInfoGatherer();
            Console.WriteLine(systemInfoGatherer.Gather());
        }
    }
}
