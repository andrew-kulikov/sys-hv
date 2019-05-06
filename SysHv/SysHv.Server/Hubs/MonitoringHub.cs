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

            Clients.All.SendAsync("sensorAdded");

            return base.OnConnectedAsync();
        }

        public async Task AddClientSensor(ClientSensorDto dto)
        {
            var clientSensor = _mapper.Map<ClientSensor>(dto);

            //await _sensorService.AddClientSensorAsync(clientSensor);

            //await Clients.User(Context.UserIdentifier).SendAsync("sensorAdded");
        }
    }
}