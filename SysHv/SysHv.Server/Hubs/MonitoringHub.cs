using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RabbitMQCommunications.Communications;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.Models;

namespace SysHv.Server.Hubs
{
    public class MonitoringHub : Hub
    {
        private OneWayReceiver<HardwareInfoDTO> _receiver;
        public MonitoringHub()
        {
            _receiver = new OneWayReceiver<HardwareInfoDTO>(new ConnectionModel(), "asd");
        }
    }
}
