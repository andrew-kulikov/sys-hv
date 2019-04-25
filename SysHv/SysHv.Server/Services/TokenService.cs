using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SysHv.Server.Configuration;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenConfiguration _tokenConfiguration;

        public TokenService(UserManager<ApplicationUser> userManager, TokenConfiguration tokenConfiguration)
        {
            _userManager = userManager;
            _tokenConfiguration = tokenConfiguration;
        }
        public string GetToken(string userName, DateTime? expires)
        {
            var user = _userManager.FindByNameAsync(userName).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            var claims = roles.Select(role => new Claim("Role", role)).ToList();

            var identity = new ClaimsIdentity(new GenericIdentity(userName, "TokenAuth"), claims);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _tokenConfiguration.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }
    }
}
