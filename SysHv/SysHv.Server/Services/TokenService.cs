using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SysHv.Server.Configuration;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services
{
    public class TokenService : ITokenService
    {
        private UserManager<ApplicationUser> _userManager;
        private TokenConfiguration _tokenConfiguration;

        public TokenService(UserManager<ApplicationUser> userManager, TokenConfiguration tokenConfiguration)
        {
            _userManager = userManager;
            _tokenConfiguration = tokenConfiguration;
        }
        public string GetToken(string userName, DateTime? expires)
        {
            var handler = new JwtSecurityTokenHandler();

            // Here, you should create or look up an identity for the userName which is being authenticated.
            // For now, just creating a simple generic identity.
            //_userManager. 
            var user = _userManager.FindByNameAsync(userName).Result;
            //var f = new UserClaimsPrincipalFactory<ApplicationUser>(_userManager, new OptionsWrapper<IdentityOptions>(new IdentityOptions())).CreateAsync(user).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            var claims = new List<Claim>();
            foreach (var role in roles)
            {
                claims.Add(new Claim("Role", role));
            }
            //var t = _userManager.GetRolesAsync(user).Result;
            var identity = new ClaimsIdentity(new GenericIdentity(userName, "TokenAuth"), claims);

            var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
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
