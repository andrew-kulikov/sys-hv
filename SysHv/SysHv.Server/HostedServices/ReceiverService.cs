using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Server.Helpers;
using SysHv.Server.Hubs;

namespace SysHv.Server.HostedServices
{
    public class ReceiverService : IHostedService, IDisposable
    {
        private readonly IConfigurationHelper _configurationHelper;
        private readonly IHubContext<MonitoringHub> _hubContext;
        private IDictionary<string, OneWayReceiver> _userReceivers;

        public ReceiverService(IHubContext<MonitoringHub> hubContext, IConfigurationHelper configurationHelper)
        {
            _hubContext = hubContext;
            _configurationHelper = configurationHelper;

            _userReceivers = new Dictionary<string, OneWayReceiver>();
        }

        public void RegisterClient(string queueName, string userId)
        {
            using (var creator = new QueueCreator(_configurationHelper.ConnectionInfo))
            {
                creator.TryCreateQueue(queueName);
            }

            var receiver = new OneWayReceiver(_configurationHelper.ConnectionInfo, queueName);
            receiver.Receive(MessageReceived);

            _userReceivers[userId] = receiver;
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body);
        
            _hubContext.Clients.All.SendAsync("UpdateReceived", JsonConvert.DeserializeObject<float>(message));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            foreach (var receiver in _userReceivers.Values)
            {
                receiver.Dispose();
            }
        }
    }
}