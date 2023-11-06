﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Web_Api.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase 
    {
        private IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody]Credential credential)
        {
            if (credential.Username == "admin" && credential.Password == "password123")
            {
                var claims = new List<Claim>
                {
                    new Claim( ClaimTypes.Name, "admin"),
                    new Claim( ClaimTypes.Email, "admin@mywebsite.com"),
                    new Claim( "Department", "HR"),
                    new Claim( "Admin", "true"),
                    new Claim( "Management", "true"),
                    new Claim( "EmploymentDate", "2023-05-01")
                };

                var expiresAt = DateTime.UtcNow.AddMinutes(90);
                return Ok(new { 
                    access_token = CreateToken(claims, expiresAt), 
                    expires_at = expiresAt,                
                });
       
            }
            ModelState.AddModelError("Unauthorized", "You are not authorized to access the endpoint.");
            return Unauthorized(ModelState);
        }

        private object CreateToken(List<Claim> claims, DateTime expireAt)
        {
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecurityKey"));

            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expireAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
