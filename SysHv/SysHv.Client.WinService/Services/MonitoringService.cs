using SysHv.Client.WinService.Gatherers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using NLog;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.Models;

namespace SysHv.Client.WinService.Services
{
    class MonitoringService
    {
        #region Constants

        private const int TimerDelay = 10000;

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
            var logged = Login().Result;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        #endregion

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var systemInfoGatherer = new HardwareInfoGatherer();
            var collectedInfo = systemInfoGatherer.Gather();
            _logger.Info(collectedInfo);
            using (var rabbitSender = new OneWaySender<HardwareInfoDTO>(new ConnectionModel(),
                new PublishProperties { ExchangeName = "", QueueName = "asd" }))
            {
                rabbitSender.Send(collectedInfo);
            }
        }

        private async Task<bool> Login()
        {
            var serverAddress = ConfigurationManager.AppSettings["ServerAddress"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(
                    JsonConvert.SerializeObject(new { Email = "123", Password = "123Qwe!" }),
                    Encoding.UTF8,
                    "application/json");

                var result = await client.PostAsync("/api/client/login", content);

                if (result.IsSuccessStatusCode)
                {
                    var resultStr = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(resultStr);
                }
            }

            return false;
        }
    }
}
