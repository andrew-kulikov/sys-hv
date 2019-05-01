using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Client.Common.Models;
using SysHv.Server.Hubs;

namespace SysHv.Server.HostedServices
{
    public class ReceiverService : IHostedService, IDisposable
    {
        private OneWayReceiver _receiver;
        private IHubContext<MonitoringHub> _hubContext;

        public ReceiverService(IHubContext<MonitoringHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void RegisterClient(string queueName)
        {
            using (var creator = new QueueCreator("localhost", "guest", "guest"))
            {
                creator.TryCreateQueue(queueName);
            }
            _receiver = new OneWayReceiver(
                new ConnectionModel("localhost", "guest", "guest"),
                queueName);
            _receiver.Receive<RuntimeInfoDTO>((a) => _hubContext.Clients.All.SendAsync("UpdateReceived", a));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            var message = Encoding.UTF8.GetString(ea.Body);
            _hubContext.Clients.All.SendAsync("UpdateReceived", message);
        }

        public void Dispose()
        {
            _receiver.Dispose();
        }
    }
}
