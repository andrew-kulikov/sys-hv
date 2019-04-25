using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> LoginClient([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return Json(new { success = false });

            var user = await _userManager.FindByEmailAsync(dto.Email);
            var userExist = await _userManager.CheckPasswordAsync(user, dto.Password);

            return Json(new { success = userExist });
        }
    }
}
