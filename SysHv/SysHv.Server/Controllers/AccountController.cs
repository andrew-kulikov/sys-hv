using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;
using SysHv.Server.Services;

namespace SysHv.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize("Bearer")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginModel)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false, false);
            if (loginResult.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                var tokenExpires = DateTime.UtcNow.AddMinutes(720);
                var token = _tokenService.GetToken(loginModel.Email, tokenExpires);

                return new JsonResult(new { success = true, token });
            }

            return new JsonResult(new { success = false });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] LoginDto loginModel)
        {
            var user = new ApplicationUser
            {
                UserName = loginModel.Email,
                Email = loginModel.Email
            };

            var registerResult = await _userManager.CreateAsync(user, loginModel.Password);

            return new JsonResult(new { success = registerResult.Succeeded });
        }
    }
}