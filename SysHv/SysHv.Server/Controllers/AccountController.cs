using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;
using SysHv.Server.Services;
using SysHv.Server.Services.Contract;

namespace SysHv.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginModel)
        {
            var loginResult =
                await _signInManager.PasswordSignInAsync(userLoginModel.Email, userLoginModel.Password, false, false);
            if (loginResult.Succeeded)
            {
                var tokenExpires = DateTime.UtcNow.AddMinutes(720);
                var token = _tokenService.GetToken(userLoginModel.Email, tokenExpires);

                return new JsonResult(new {token});
            }

            return Unauthorized("User with this email and password does not exist");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserLoginDto userLoginModel)
        {
            var user = new ApplicationUser
            {
                UserName = userLoginModel.Email,
                Email = userLoginModel.Email
            };
            var success = false;
            try
            {
                var registerResult = await _userManager.CreateAsync(user, userLoginModel.Password);
                success = registerResult.Succeeded;
            }
            catch (Exception e)
            {
                return new JsonResult(new {success, message = e.Message, inner = e.InnerException.Message});
            }

            return new JsonResult(new {success});
        }
    }
}