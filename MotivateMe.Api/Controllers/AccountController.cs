using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MotivateMe.Api.MotivateMeContext;
using MotivateMe.Api.MotivateMeContext.Jwt;

namespace MotivateMe.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly JwtSettings _jwtSettings;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,
            IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                return BadRequest(ModelState);
            }

            return BadRequest();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _userManager.CheckPasswordAsync(user, model.Password);

                if (!result)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return BadRequest(ModelState);
                }

                return Ok(GenerateJwt(user));
            }
            return BadRequest(model);
        }

        private string GenerateJwt(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
