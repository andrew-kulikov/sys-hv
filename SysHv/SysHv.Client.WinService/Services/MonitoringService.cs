using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
            var libDirectory = ConfigurationManager.AppSettings["SensorExtensionsPath"];
            Type sensorType = null;

            foreach (var sensorDirectory in Directory.GetDirectories(libDirectory))
            foreach (var sensorPath in Directory.GetFiles(sensorDirectory, sensor.Contract + ".dll"))
            {
                var assembly = Assembly.LoadFile(sensorPath);

                sensorType = assembly.GetTypes().FirstOrDefault(a => a.Name == sensor.Contract);
            }

            if (sensorType != null)
            {
                var sensorInstance = Activator.CreateInstance(sensorType);
                Console.WriteLine($"{sensorType.Namespace} : {sensorType.Name}");
                var collect = sensorType.GetMethod("Collect");
                var result = collect?.Invoke(sensorInstance, new object[] { });
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Not found");
            }


            return (sender, args) =>
            {
                var returnType = sensor.ReturnType == "float"
                    ? typeof(float)
                    : Type.GetType("SysHv.Client.Common." + sensor.ReturnType);
                var senderType = typeof(OneWaySender).MakeGenericType(typeof(RuntimeInfoDTO));

                var rabbitSender = Activator.CreateInstance(senderType, new ConnectionModel(),
                    new PublishProperties {ExchangeName = "", QueueName = queueName});

                try
                {
                    object result = null;
                    if (sensorType != null)
                    {
                        var sensorInstance = Activator.CreateInstance(sensorType);
                        var collect = sensorType.GetMethod("Collect");

                        result = collect?.Invoke(sensorInstance, new object[] { });

                        Console.WriteLine($"{sensorType.Namespace} : {sensorType.Name}");
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Not found");
                        return;
                    }

                    var sendMethod = senderType.GetMethod("Send");
                    sendMethod?.Invoke(rabbitSender, new[] { new RuntimeInfoGatherer().Gather() });
                }
                finally
                {
                    (rabbitSender as IDisposable)?.Dispose();
                }
            };
        }

        private void LoginTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var loginResponse = Login().Result;
            if (loginResponse != null && loginResponse.Success)
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
                    JsonConvert.SerializeObject(new { email = "123", password = "123Qwe!", ip = "127.0.0.1"}),//"178.122.194.35" }),
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
            /*
            var curAssembly = Assembly.GetEntryAssembly();
            foreach (var type in curAssembly.GetTypes())
            {
                if (type.IsClass)
                Console.WriteLine(type);
            }*/

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