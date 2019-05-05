using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SysHv.Server.Services;

namespace SysHv.Server.Hubs
{
    public class MonitoringHub : Hub
    {
        private readonly ISensorService _sensorService;
        private readonly IClientService _clientService;

        public MonitoringHub(ISensorService sensorService, IClientService clientService)
        {
            _sensorService = sensorService;
            _clientService = clientService;
        }

        [Authorize("Bearer")]
        public string AddSensor()
        {
            var user = Context.User.Identity.Name;
            return user;
        }
    }
}
