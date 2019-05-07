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
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Server.DAL;
using SysHv.Server.Helpers;
using SysHv.Server.Hubs;
using SysHv.Server.Services;

namespace SysHv.Server.HostedServices
{
    public class ReceiverService : IHostedService, IDisposable
    {
        private readonly IConfigurationHelper _configurationHelper;
        private readonly IHubContext<MonitoringHub> _hubContext;
        private readonly DbContextOptions<ServerDbContext> _options;
        private readonly IDictionary<string, IDictionary<int, OneWayReceiver>> _userReceivers;

        public ReceiverService(IHubContext<MonitoringHub> hubContext, IConfigurationHelper configurationHelper, DbContextOptions<ServerDbContext> clientService)
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
             

            if (type == "HardwareInfo")
            {
                var info = JsonConvert.DeserializeObject<HardwareInfoDTO>(message);
                var clientId = int.Parse(ea.BasicProperties.AppId);

                using (var context = new ServerDbContext(_options))
                {
                    var client = context.Clients.FirstOrDefault(c => c.Id == clientId);
                    if (client != null)
                    {
                        client.HardwareInfo = message;
                        context.SaveChanges();
                    }
                }
            }

            _hubContext.Clients.All.SendAsync("UpdateReceived", JsonConvert.DeserializeObject(message));
        }
    }
}