using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;
using SysHv.Server.Services;

namespace SysHv.Server.Hubs
{
    [Authorize("Bearer")]
    public class MonitoringHub : Hub
    {
        internal static readonly IDictionary<string, ICollection<string>> Connections =
            new Dictionary<string, ICollection<string>>();

        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly ISensorService _sensorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MonitoringHub(ISensorService sensorService, IClientService clientService,
            UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _sensorService = sensorService;
            _clientService = clientService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public override Task OnConnectedAsync()
        {
            var user = Context.User.Identity.Name;

            if (!Connections.Keys.Contains(user))
                Connections[user] = new List<string>();
            Connections[user].Add(Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = Context.User.Identity.Name;

            Connections[user].Remove(Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddClientSensor(ClientSensorDto dto)
        {
            var user = Context.User.Identity.Name;
            var client = await _clientService.GetClientByIdAsync(dto.ClientId);
            /*var clientSensor = _mapper.Map<ClientSensor>(dto);

           await _sensorService.AddClientSensorAsync(clientSensor);*/
            using (var sender = new RPCSender(new ConnectionModel(client.Ip, "vasya", "123456"),
                new PublishProperties {QueueName = "rpc", ExchangeName = ""}))
            {
                var res = await sender.Call<string, string>("sdf");
                await Clients.Clients(Connections[user] as IReadOnlyList<string>).SendAsync("sensorAdded", res);
            }
        }
    }
}