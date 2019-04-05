using SysHv.Client.TopShelfService.Gatherers;
using System;
using System.Timers;
using NLog;

namespace SysHv.Client.TopShelfService.Services
{
    class MonitoringService
    {
        #region Constants

        private const int TimerDelay = 1000;

        #endregion

        #region Private Fields

        private readonly Timer _timer;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        public MonitoringService()
        {
            _timer = new Timer(TimerDelay);
            _timer.AutoReset = true;
            _timer.Elapsed += TimerElapsed;
            _timer.Enabled = true;
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
            var systemInfoGatherer = new RuntimeInfoGatherer();
            _logger.Info(systemInfoGatherer.Gather());
            Console.WriteLine(systemInfoGatherer.Gather());
        }
    }
}
