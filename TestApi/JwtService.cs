using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestApi
{
    public class JwtService : IJwtService
    {
        public string Generate(IdentityUser user)
        {
            var sk = Encoding.UTF8.GetBytes(
                "my_not_very_strong_secret_key_for_encryption");
            var ssk = new SymmetricSecurityKey(sk);

            var signingCredentials = new SigningCredentials(
                ssk, SecurityAlgorithms.HmacSha256Signature);

            var claims = GetClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "TestApi",
                Audience = "TestApi",
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            var a = new JwtSecurityTokenHandler();
            var s = a.CreateToken(descriptor);
            var r = a.WriteToken(s);

            return r;
        }

        private IEnumerable<Claim> GetClaims(IdentityUser user)
        {
            var list = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Teacher"),
            };

            return list;
        }
    }
}
