using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Client.Common.Models;
using SysHv.Client.WinService.Gatherers;
using Decoder = RabbitMQCommunications.Communications.Decoding.Decoder;

namespace SysHv.Client.WinService.Services
{
    internal class MonitoringService
    {
        #region Constructors

        public MonitoringService()
        {
            _sensorTimers = new List<Timer>();
            _locker = new object();

            _loginTimer = new Timer(LoginTimerDelay);
            _loginTimer.AutoReset = true;
            _loginTimer.Elapsed += LoginTimerElapsed;
        }

        #endregion


        private ElapsedEventHandler GetTimerElapsed(SensorDto sensor)
        {
            return (sender, args) =>
            {
                var systemInfoGatherer = new HardwareInfoGatherer();
                var runtimeGatherer = new RuntimeInfoGatherer();

                var collectedInfo = systemInfoGatherer.Gather();
                var collectedRuntimeInfo = runtimeGatherer.Gather();

                //_logger.Info(collectedInfo);

                using (var rabbitSender = new OneWaySender<RuntimeInfoDTO>(new ConnectionModel(),
                    new PublishProperties {ExchangeName = "", QueueName = queueName}))
                {
                    rabbitSender.Send(collectedRuntimeInfo);
                    Console.WriteLine(string.Join("",
                        collectedRuntimeInfo.CouLoad.Select(c => $"{c.Name}: {c.Value}; ")));
                }
            };
        }

        private void LoginTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var loginResponse = Login().Result;
            if (loginResponse.Success)
            {
                _loginTimer.Enabled = false;

                lock (_locker)
                {
                    queueName = loginResponse.Message;
                }

                LaunchSensors(loginResponse.Sensors);
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
                    JsonConvert.SerializeObject(new {email = "123", password = "123Qwe!", ip = "178.122.194.35"}),
                    Encoding.UTF8,
                    "application/json");

                var result = await client.PostAsync("/api/client/login", content);

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

        private void LaunchSensors(IEnumerable<SensorDto> sensors)
        {
            foreach (var sensor in sensors)
            {
                var timer = new Timer(TimerDelay);
                timer.AutoReset = true;
                timer.Elapsed += GetTimerElapsed(sensor);
                var sensorType = sensor.Contract;
                Console.WriteLine(sensorType);
                timer.Enabled = true;
                _sensorTimers.Add(timer);
            }
        }

        #region Constants

        private const int TimerDelay = 5000;
        private const int LoginTimerDelay = 5000;
        private readonly object _locker;

        private string queueName;

        #endregion

        #region Private Fields

        private readonly IList<Timer> _sensorTimers;
        private readonly Timer _loginTimer;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Public Methods

        public void Start()
        {
            var libDirectory = ConfigurationManager.AppSettings["SensorExtensionsPath"];
 
            foreach (var sensorPath in Directory.GetFiles(libDirectory, "*.dll"))
            {
                Console.WriteLine(sensorPath);
            }
          
            Console.ReadLine();
            _loginTimer.Enabled = true;
        }

        public void Stop()
        {
            _loginTimer.Enabled = false;
            foreach (var timer in _sensorTimers) timer.Enabled = false;
        }

        #endregion
    }
}