using SysHv.Client.WinService.Gatherers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using NLog;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Client.Common.Models;
using Decoder = RabbitMQCommunications.Communications.Decoding.Decoder;

namespace SysHv.Client.WinService.Services
{
    class MonitoringService
    {
        #region Constants

        private const int TimerDelay = 5000;
        private const int LoginTimerDelay = 5000;
        private object _locker;

        private string queueName;

        #endregion

        #region Private Fields

        private readonly Timer _timer;
        private readonly Timer _loginTimer;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        public MonitoringService()
        {
            _locker = new object();

            _timer = new Timer(TimerDelay);
            _timer.AutoReset = true;
            _timer.Elapsed += TimerElapsed;

            _loginTimer = new Timer(LoginTimerDelay);
            _loginTimer.AutoReset = true;
            _loginTimer.Elapsed += LoginTimerElapsed;
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            Console.ReadLine();
            _loginTimer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Enabled = false;
            _loginTimer.Enabled = false;
        }

        #endregion

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var systemInfoGatherer = new HardwareInfoGatherer();
            var runtimeGatherer = new RuntimeInfoGatherer();

            var collectedInfo = systemInfoGatherer.Gather();
            var collectedRuntimeInfo = runtimeGatherer.Gather();

            //_logger.Info(collectedInfo);

            using (var rabbitSender = new OneWaySender(new ConnectionModel(),
                new PublishProperties { ExchangeName = "", QueueName = queueName }))
            {
                rabbitSender.Send(collectedRuntimeInfo);
                Console.WriteLine(string.Join("", collectedRuntimeInfo.CouLoad.Select(c => $"{c.Name}: {c.Value}; ")));
            }
        }

        private void LoginTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var loginResponse = Login().Result;
            if (loginResponse.Success)
            {
                _timer.Enabled = true;
                _loginTimer.Enabled = false;

                lock (_locker)
                {
                    queueName = loginResponse.Message;
                }
            }
        }

        private async Task<Response> Login()
        {
            var serverAddress = ConfigurationManager.AppSettings["ServerAddress"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverAddress);
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(
                    JsonConvert.SerializeObject(new { email = "123", password = "123Qwe!", ip = "178.122.194.35" }),
                    Encoding.UTF8,
                    "application/json");

                var  result = await client.PostAsync("/api/client/login", content);

                if (result.IsSuccessStatusCode)
                {
                    var resultStr = await result.Content.ReadAsStringAsync();
                    var response = Decoder.Decode<Response>(resultStr);
                    Console.WriteLine(resultStr);

                    return response;
                }
            }

            return null;
        }

        private void LaunchSensors(IEnumerable<string> sensors)
        {

        }
    }
}
