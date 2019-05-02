using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
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
using SysHv.Client.WinService.Helpers;
using Decoder = RabbitMQCommunications.Communications.Decoding.Decoder;
using Timer = System.Timers.Timer;

namespace SysHv.Client.WinService.Services
{
    internal class MonitoringService
    {
        private const int TimerDelay = 5000;

        private readonly IList<Timer> _sensorTimers;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private string _queueName;
        private readonly IList<Assembly> _assemblies;


        public MonitoringService()
        {
            _assemblies = new List<Assembly>();
            new HardwareInfoGatherer().Gather();
            _sensorTimers = new List<Timer>();
        }

        private ElapsedEventHandler GetTimerElapsed(SensorDto sensor)
        {
            var sensorType = _assemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(a => a.Name == sensor.Contract);

            return (sender, args) =>
            {
                using (var rabbitSender = new OneWaySender(new ConnectionModel(),
                    new PublishProperties { ExchangeName = "", QueueName = _queueName }))
                {
                    object result = null;
                    if (sensorType != null)
                    {
                        var sensorInstance = Activator.CreateInstance(sensorType);
                        var collect = sensorType.GetMethod("Collect");

                        result = collect?.Invoke(sensorInstance, new object[] { });

                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Not found");
                        return;
                    }

                    rabbitSender.Send(new SensorResponse
                    {
                        ClientId = 1,
                        SensorId = sensor.Id,
                        Value = result
                    });
                }
            };
        }

        private void LoginTimerElapsed()
        {
            var loginResponse = Login().Result;

            if (loginResponse == null || !loginResponse.Success)
            {
                Thread.Sleep(TimerDelay);
                LoginTimerElapsed();
            }

            _queueName = loginResponse.Message;

            LaunchSensors(loginResponse.Sensors);
        }

        private async Task<Response> Login()
        {
            var serverAddress = ConfigurationManager.AppSettings["ServerAddress"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(serverAddress);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(
                    JsonConvert.SerializeObject(ConfigurationHelper.LoginDto),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage result;
                try
                {
                    result = await client.PostAsync("/api/client/login", content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }

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
                var timer = new Timer(TimerDelay) { AutoReset = true };
                timer.Elapsed += GetTimerElapsed(sensor);

                timer.Enabled = true;
                _sensorTimers.Add(timer);
            }
        }

        public void Start()
        {
            Console.WriteLine("Enter something...");
            Console.ReadLine();

            var libDirectory = ConfigurationManager.AppSettings["SensorExtensionsPath"];
            foreach (var sensorDirectory in Directory.GetDirectories(libDirectory))
                foreach (var sensorPath in Directory.GetFiles(sensorDirectory, "*Sensor*.dll"))
                    _assemblies.Add(Assembly.LoadFile(sensorPath));

            LoginTimerElapsed();
        }

        public void Stop()
        {
            foreach (var timer in _sensorTimers) timer.Enabled = false;
        }
    }
}