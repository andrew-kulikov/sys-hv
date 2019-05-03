﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using NLog;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Client.Common.Models;
using SysHv.Client.WinService.Communication;

namespace SysHv.Client.WinService.Services
{
    internal class MonitoringService
    {
        private const int TimerDelay = 5000;
        private readonly IList<Assembly> _assemblies;
        private readonly ServerRestClient _restClient;
        private readonly IList<object> _sensorInstances;

        private readonly IList<Timer> _sensorTimers;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private string _queueName;


        public MonitoringService()
        {
            _assemblies = new List<Assembly>();
            _sensorTimers = new List<Timer>();
            _sensorInstances = new List<object>();
            _restClient = new ServerRestClient();
        }

        private ElapsedEventHandler GetTimerElapsed(SensorDto sensor)
        {
            var sensorType = _assemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(a => a.Name == sensor.Contract);

            if (sensorType == null) return null;

            var sensorInstance = Activator.CreateInstance(sensorType);
            _sensorInstances.Add(sensorInstance);

            return (sender, args) =>
            {
                using (var rabbitSender = new OneWaySender(new ConnectionModel(),
                    new PublishProperties { ExchangeName = "", QueueName = _queueName }))
                {
                    var collect = sensorType.GetMethod("Collect");
                    var result = collect?.Invoke(sensorInstance, new object[] { });

                    Console.WriteLine(result);

                    rabbitSender.Send(new SensorResponse
                    {
                        ClientId = 1,
                        SensorId = sensor.Id,
                        Value = result
                    });
                }
            };
        }

        private async Task StartServerConnection()
        {
            while (true)
            {
                var loginResponse = await _restClient.Login();

                if (loginResponse != null && loginResponse.Success)
                {
                    _queueName = loginResponse.Message;
                    LaunchSensors(loginResponse.Sensors);

                    break;
                }

                await Task.Delay(TimerDelay);
            }
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
            var libDirectory = ConfigurationManager.AppSettings["SensorExtensionsPath"];

            foreach (var sensorDirectory in Directory.GetDirectories(libDirectory))
                foreach (var sensorPath in Directory.GetFiles(sensorDirectory, "*Sensor*.dll"))
                    _assemblies.Add(Assembly.LoadFile(sensorPath));

            Task.Run(StartServerConnection);
        }

        public void Stop()
        {
            foreach (var sensorInstance in _sensorInstances) (sensorInstance as IDisposable)?.Dispose();
            foreach (var timer in _sensorTimers) timer.Enabled = false;
        }
    }
}