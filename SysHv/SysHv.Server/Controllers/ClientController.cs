using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;
using SysHv.Server.Services;

namespace SysHv.Server.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClientService _clientService;

        public ClientController(UserManager<ApplicationUser> userManager, IClientService clientService)
        {
            _userManager = userManager;
            _clientService = clientService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginClient([FromBody] ClientLoginDto dto)
        {
            if (!ModelState.IsValid) return Json(new { success = false });

            var user = await _userManager.FindByEmailAsync(dto.Email);
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            var clientExist = await _clientService.ClientExistAsync(dto.Ip, user.Id);

            var queue = clientExist && passwordCorrect ? dto.Ip : null;

            return Json(new { success = clientExist, queue });
        }

        [Route("register")]
        [HttpPost]
        [Authorize("Bearer")]
        public async Task<IActionResult> RegisterClient([FromBody] ClientRegisterDto dto)
        {
            if (!ModelState.IsValid || !User.Identity.IsAuthenticated) return Json(new { success = false });

            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var client = new DAL.Models.Client
            {
                Ip = dto.Ip,
                Name = dto.Name,
                Description = dto.Description
            };
            await _clientService.AddClientAsync(client, user);

            return Json(new { success = true });
        }
    }
}
