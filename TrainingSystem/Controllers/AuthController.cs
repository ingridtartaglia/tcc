using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TrainingSystem.Auth;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Permite que o usuário se autentique para pegar novos jwt tokens
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var identity = GetClaimsIdentity(loginViewModel.Email, loginViewModel.Password);
            if (identity == null) {
                ModelState.AddModelError("login_failure", "Invalid username or password.");
                return BadRequest(ModelState);
            }

            var jwt = GenerateJwt(identity, _jwtFactory, loginViewModel.Email, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        [HttpGet("user")]
        public AppUser GetCurrentUser() {
            var user = _userManager.GetUserAsync(User).Result;
            return user;
        }

        private ClaimsIdentity GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            // get the user to verifty
            var userToVerify = _userManager.FindByEmailAsync(userName).Result;

            if (userToVerify == null) return null;

            // check the credentials
            var result = _signInManager.CheckPasswordSignInAsync(userToVerify, password, false).Result;
            if (result.Succeeded)
            {
                var role = _userManager.GetRolesAsync(userToVerify).Result.FirstOrDefault();
                return _jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, role);
            }

            // Credentials are invalid, or account doesn't exist
            return null;
        }

        public static string GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = jwtFactory.GenerateEncodedToken(userName, identity).Result,
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}