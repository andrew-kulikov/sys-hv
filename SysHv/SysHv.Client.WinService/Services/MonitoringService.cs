using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Threading.Tasks;
using System.Timers;
using NLog;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.Exceptions;
using RabbitMQCommunications.Communications.HelpStuff;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Client.Common.Models;
using SysHv.Client.WinService.Communication;
using SysHv.Client.WinService.Gatherers;
using SysHv.Client.WinService.Helpers;

namespace SysHv.Client.WinService.Services
{
    internal class MonitoringService
    {
        private readonly IList<Assembly> _assemblies;
        private readonly ServerRestClient _restClient;
        private readonly IList<object> _sensorInstances;
        private readonly RPCReceiver _receiver;

        private readonly IList<Timer> _sensorTimers;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private string _queueName;


        public MonitoringService()
        {
            using (var creator = new QueueCreator(new ConnectionModel()))
            {
                creator.TryCreateQueue("rpc", false, false, false, null);
            }
            _receiver = new RPCReceiver(new ConnectionModel(), new PublishProperties { QueueName = "rpc", ExchangeName = "" });
            _assemblies = new List<Assembly>();
            _sensorTimers = new List<Timer>();
            _sensorInstances = new List<object>();
            _restClient = new ServerRestClient();
        }

        private ElapsedEventHandler GetTimerElapsed(SensorDto sensor)
        {
            var sensorType = _assemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(a => a.Name == sensor.Contract);

            if (sensorType == null) return null;

            var sensorInstance = Activator.CreateInstance(sensorType, sensor);
            _sensorInstances.Add(sensorInstance);

            return (sender, args) =>
            {
                using (var rabbitSender = new OneWaySender(new ConnectionModel(),
                    new PublishProperties { ExchangeName = "", QueueName = _queueName }))
                {
                    var collect = sensorType.GetMethod("Collect");
                    var result = collect?.Invoke(sensorInstance, new object[] { });

                    Console.WriteLine($"{sensor.Name} : {result}");

                    rabbitSender.Send(new SensorResponse
                    {
                        ClientId = ConfigurationHelper.Id,
                        SensorId = sensor.Id,
                        Value = result,
                        Time = DateTime.Now,
                        UserEmail = ConfigurationHelper.Email
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
                    
                    _receiver.StartListen<string, string>(s =>
                    {
                        Console.WriteLine(s);
                        return "good";
                    });
                    LaunchSensors(loginResponse.Sensors);
                    SendHardwareInfo();

                    break;
                }

                await Task.Delay(ConfigurationHelper.ReconnectionInterval);
            }
        }

        private void SendHardwareInfo()
        {
            var gatherer = new HardwareInfoGatherer();
            var info = gatherer.Gather();

            using (var rabbitSender = new OneWaySender(new ConnectionModel(),
                new PublishProperties {ExchangeName = "", QueueName = _queueName}))
            {
                rabbitSender.Send(info, "HardwareInfo", ConfigurationHelper.Id.ToString());
            }
        }

        private void LaunchSensors(IEnumerable<SensorDto> sensors)
        {
            foreach (var sensor in sensors)
            {
                var timer = new Timer(sensor.Interval) { AutoReset = true };

                timer.Elapsed += GetTimerElapsed(sensor);
                timer.Enabled = true;

                _sensorTimers.Add(timer);
            }
        }

        public void Start()
        {
            Console.ReadLine();

            var libDirectory = ConfigurationManager.AppSettings["SensorExtensionsPath"];

            foreach (var sensorDirectory in Directory.GetDirectories(libDirectory))
                foreach (var sensorPath in Directory.GetFiles(sensorDirectory, "*Sensor*.dll"))
                    _assemblies.Add(Assembly.LoadFile(sensorPath));

            Task.Run(StartServerConnection);
        }

        public void Stop()
        {
            foreach (var timer in _sensorTimers) timer.Enabled = false;
            foreach (var sensorInstance in _sensorInstances) (sensorInstance as IDisposable)?.Dispose();
            _receiver.Dispose();
        }
    }
}