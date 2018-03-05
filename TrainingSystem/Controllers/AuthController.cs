using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody] LoginViewModel loginViewModel)
        {
            var appUser = Authenticate(loginViewModel);

            if (appUser != null)
            {
                var tokenString = BuildToken(appUser);
                return Ok(new { token = tokenString });
            }

            return Unauthorized();
        }

        private AppUser Authenticate(LoginViewModel loginViewModel)
        {
            AppUser appUser = _userManager.FindByEmailAsync(loginViewModel.Email).Result;

            if (appUser == null) return null;

            if (_userManager.CheckPasswordAsync(appUser, loginViewModel.Password).Result)
            {
                return appUser;
            }
            return null;
        }

        private string BuildToken(AppUser appUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}