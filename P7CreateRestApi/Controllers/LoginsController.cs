using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using P7CreateRestApi.ClassDto;   
using P7CreateRestApi.Entities;   
using Serilog;

namespace P7CreateRestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class LoginsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;

        public LoginsController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

      
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] UserDto dto, CancellationToken ct)
        {
            
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing is not null)
                return Conflict(new { message = "Email already registered." });

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                Fullname = dto.Fullname
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    message = "Registration failed.",
                    errors = result.Errors.Select(e => new { code = e.Code, description = e.Description })
                });
            }

            Log.Information("User registered: {Email}", user.Email);

            
            return Created($"/api/logins/{Uri.EscapeDataString(user.Email!)}", new
            {
                message = "User registered successfully."
            });
        }

       
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized(new { message = "Invalid email or password." });

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!, dto.Password, isPersistent: false, lockoutOnFailure: true);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid email or password." });

            var (token, expiresAtUtc) = GenerateJwtToken(user);

            Log.Information("User logged in: {Email}", user.Email);

            return Ok(new
            {
                token,
                expiresAtUtc,
                email = user.Email,
                fullname = user.Fullname
            });
        }

        private (string token, DateTime expiresAtUtc) GenerateJwtToken(User user)
        {
            var keyValue = _config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(keyValue))
                keyValue = "CHANGE-ME-in-secrets-or-appsettings"; 

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expires = DateTime.UtcNow.AddHours(1);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("fullname", user.Fullname ?? string.Empty),
            };

            var token = new JwtSecurityToken(
                issuer: string.IsNullOrWhiteSpace(issuer) ? null : issuer,
                audience: string.IsNullOrWhiteSpace(audience) ? null : audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}
