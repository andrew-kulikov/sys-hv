using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;
using SysHv.Server.Services;

namespace SysHv.Server.Hubs
{
    [Authorize("Bearer")]
    public class MonitoringHub : Hub
    {
        private readonly IClientService _clientService;
        private static readonly IDictionary<string, ICollection<string>> Connections = new Dictionary<string, ICollection<string>>();
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
            var clientSensor = _mapper.Map<ClientSensor>(dto);

            await _sensorService.AddClientSensorAsync(clientSensor);

            await Clients.Clients(Connections[user] as IReadOnlyList<string>).SendAsync("sensorAdded");
        }
    }
}