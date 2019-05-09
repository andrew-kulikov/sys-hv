using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Server.DAL;
using SysHv.Server.DAL.Models;
using SysHv.Server.Helpers;
using SysHv.Server.Hubs;

namespace SysHv.Server.HostedServices
{
    public class ReceiverService : IHostedService, IDisposable
    {
        private readonly IConfigurationHelper _configurationHelper;
        private readonly IHubContext<MonitoringHub> _hubContext;
        private readonly DbContextOptions<ServerDbContext> _options;
        private readonly IDictionary<string, IDictionary<int, OneWayReceiver>> _userReceivers;

        public ReceiverService(IHubContext<MonitoringHub> hubContext, IConfigurationHelper configurationHelper,
            DbContextOptions<ServerDbContext> clientService)
        {
            _hubContext = hubContext;
            _configurationHelper = configurationHelper;
            _options = clientService;

            _userReceivers = new Dictionary<string, IDictionary<int, OneWayReceiver>>();
        }

        public void Dispose()
        {
            foreach (var receiver in _userReceivers.SelectMany(v => v.Value.Values)) receiver.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var receiver in _userReceivers.SelectMany(v => v.Value.Values)) receiver.Dispose();
            return Task.CompletedTask;
        }

        public void RegisterClient(int clientId, string userId)
        {
            var queueName = clientId.ToString();
            using (var creator = new QueueCreator(_configurationHelper.ConnectionInfo))
            {
                creator.TryCreateQueue(queueName);
            }

            var receiver = new OneWayReceiver(_configurationHelper.ConnectionInfo, queueName);
            receiver.Receive(MessageReceived);

            if (!_userReceivers.ContainsKey(userId)) _userReceivers[userId] = new Dictionary<int, OneWayReceiver>();
            _userReceivers[userId][clientId] = receiver;
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body);
            var type = ea.BasicProperties.Type;

            WriteLog(message);

            if (type == "HardwareInfo")
            {
                var clientId = int.Parse(ea.BasicProperties.AppId);

                SaveHardwareInfo(clientId, message);
                return;
            }

            var messageDecoded = JsonConvert.DeserializeObject<SensorResponse>(message);

            if (messageDecoded != null && MonitoringHub.Connections.ContainsKey(messageDecoded.UserEmail))
                _hubContext.Clients
                    .Clients(MonitoringHub.Connections[messageDecoded.UserEmail] as IReadOnlyList<string>)
                    .SendAsync("UpdateReceived", JsonConvert.DeserializeObject(message));
        }

        private void WriteLog(string message)
        {
            var sensorResponse = JsonConvert.DeserializeObject<SensorResponse>(message);

            NumericSensorDto sensorValue = null;
            if (sensorResponse != null) sensorValue = (sensorResponse.Value as JObject)?.ToObject<NumericSensorDto>();

            if (sensorValue == null) return;

            using (var context = new ServerDbContext(_options))
            {
                var log = new SensorLog
                {
                    ClientSensorId = sensorResponse.SensorId,
                    Status = sensorValue.Status,
                    Time = sensorResponse.Time,
                    LogJson = JsonConvert.SerializeObject(sensorValue)
                };
                context.SensorLogs.Add(log);
                context.SaveChanges();
            }
        }

        private void SaveHardwareInfo(int clientId, string hardwareInfo)
        {
            using (var context = new ServerDbContext(_options))
            {
                var client = context.Clients.FirstOrDefault(c => c.Id == clientId);
                if (client != null)
                {
                    client.HardwareInfo = hardwareInfo;
                    context.SaveChanges();
                }
            }
        }
    }
}