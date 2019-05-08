using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly ISensorService _sensorService;
        private readonly IMapper _mapper;
        private readonly ReceiverService _receiver;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientController(UserManager<ApplicationUser> userManager, IClientService clientService,
            ISensorService sensorService,
            IHostedServiceAccessor<ReceiverService> receiver,
            IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _clientService = clientService;
            _sensorService = sensorService;
            _receiver = receiver.Service ?? throw new ArgumentNullException(nameof(receiver));
        }

        /// <summary>
        ///     Login client computer. Creates server session for client and returns queue name
        ///     in which client should push all sensor data.
        /// </summary>
        /// <param name="dto">Client login model</param>
        /// <returns>
        ///     {
        ///     Message: queue to push data
        ///     Sensors: list of created sensors to activate for client
        ///     Success: is client exist
        ///     }
        /// </returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginClient([FromBody] ClientLoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(new Response { Success = false, Message = "Model invalid" });

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user.PasswordHash != dto.PasswordHash)
                return Json(new Response { Success = false, Message = $"Wrong password. Expected {user.PasswordHash}" });

            var clientExist = await _clientService.ClientIdExistAsync(dto.Id, user.Id);
            var client = await _clientService.GetClientByIdAsync(dto.Id);

            if (!clientExist) return NotFound(new Response { Success = false, Message = "Client does not exist" });

            var queue = dto.Id.ToString();
            _receiver.RegisterClient(client.Id, user.Id);

            var sensors = await _sensorService.GetClientSensorsAsync(client.Id);
            var sensorDtos = sensors.Select(s => s.ToSensorDto());

            return Json(new Response { Message = queue, Success = true, Sensors = sensorDtos });
        }
 
        [HttpPost]
        [Route("register")]
        [Authorize("Bearer")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest("Wrong data format");

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var client = new DAL.Models.Client
            {
                Ip = dto.Ip,
                Name = dto.Name,
                Description = dto.Description
            };
            await _clientService.AddClientAsync(client, user);

            return Ok();
        }

        [HttpGet]
        [Authorize("Bearer")]
        public async Task<ActionResult<ICollection<ClientDto>>> GetAllClients()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var clients = await _clientService.GetAdminClientsAsync(user.Id);

            return _mapper.Map<List<DAL.Models.Client>, List<ClientDto>>(clients);
        }
    }
}