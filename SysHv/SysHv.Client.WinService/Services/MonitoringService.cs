using SysHv.Client.WinService.Gatherers;
using System;
using System.Collections.Generic;
using System.Timers;
using NLog;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;

namespace SysHv.Client.WinService.Services
{
    class MonitoringService
    {
        #region Constants

        private const int TimerDelay = 5000;

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
            var collectedInfo = systemInfoGatherer.Gather();
            _logger.Info(collectedInfo);
            using (var rabbitSender = new OneWaySender<string>(new ConnectionModel(), new PublishProperties { ExchangeName = "", QueueName = "asd" }))
            {
                rabbitSender.Send(collectedInfo);
            }
            Console.WriteLine(systemInfoGatherer.Gather());
        }
    }
}
