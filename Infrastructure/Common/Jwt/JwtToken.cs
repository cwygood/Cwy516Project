using Domain.Interfaces;
using Infrastructure.Configurations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Common.Jwt
{
    public class JwtToken : IJwtToken
    {
        private readonly JwtConfiguration _jwtConfig;
        public JwtToken(JwtConfiguration jwtConfig)
        {
            this._jwtConfig = jwtConfig;
        }
        public string GetToken(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtConfig.SigningKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var cusClaims = new List<Claim>()
            {
                // 令牌颁发时间
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                 // 过期时间 100秒
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(this._jwtConfig.Expires)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss,this._jwtConfig.Issuer), // 签发者
                new Claim(JwtRegisteredClaimNames.Aud,this._jwtConfig.Audience) // 接收者
            };
            cusClaims.AddRange(claims);
            claims = cusClaims.ToArray();
            var token = new JwtSecurityToken(
                issuer: this._jwtConfig.Issuer,
                audience: this._jwtConfig.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(this._jwtConfig.Expires),
                signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
