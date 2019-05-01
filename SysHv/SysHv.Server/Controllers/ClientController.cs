using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SysHv.Client.Common.DTOs;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;
using SysHv.Server.Helpers;
using SysHv.Server.HostedServices;
using SysHv.Server.Services;

namespace SysHv.Server.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ReceiverService _receiver;
        private readonly ISensorService _sensorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(UserManager<ApplicationUser> userManager, IClientService clientService,
            ISensorService sensorService,
            IHostedServiceAccessor<ReceiverService> receiver)
        {
            _userManager = userManager;
            _clientService = clientService;
            _sensorService = sensorService;
            _receiver = receiver.Service ?? throw new ArgumentNullException(nameof(receiver));
        }


        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginClient([FromBody] ClientLoginDto dto)
        {
            if (!ModelState.IsValid) return Json(new {success = false});

            var user = await _userManager.FindByEmailAsync(dto.Email);

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            var clientExist = await _clientService.ClientExistAsync(dto.Ip, user.Id);
            var client = await _clientService.GetClientByIpAsync(dto.Ip);
            var success = passwordCorrect && clientExist;

            var queue = success ? dto.Ip : null;

            if (success) _receiver.RegisterClient(queue);

            var sensors = await _sensorService.GetClientSensorsAsync(client.Id);
            var sensorDtos = sensors.Select(s => s.ToSensorDto());

            return Json(new Response {Message = queue, Success = success, Sensors = sensorDtos});
        }

        [Route("register")]
        [HttpPost]
        [Authorize("Bearer")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterDto dto)
        {
            if (!ModelState.IsValid || !User.Identity.IsAuthenticated) return Json(new {success = false});

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var client = new DAL.Models.Client
            {
                Ip = dto.Ip,
                Name = dto.Name,
                Description = dto.Description
            };
            await _clientService.AddClientAsync(client, user);

            return Json(new {success = true});
        }
    }
}