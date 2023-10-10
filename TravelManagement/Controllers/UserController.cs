using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TravelManagement.Interfaces;
using TravelManagement.Models;

namespace TravelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersService _userService;

        public UserController(IConfiguration configuration, IUsersService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsersClass user)
        {
            try
            {

                user.Role = "TravelAgent";

                // Register the user.
                var registeredUser = await _userService.RegisterAsyn(user);

                return Ok(new { Message = "Registration successful", User = registeredUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Registration failed", Error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsersClass model)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(model.Username, model.Password);

                if (user == null)
                {
                    return Unauthorized(new { Message = "Invalid username or password" });
                }

                // Create JWT token for authenticated user.
                var token = GenerateJwtToken(user);

                return Ok(new { Message = "Login successful", Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Login failed", Error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-dashboard")]
        public IActionResult AdminDashboard()
        {
            return Ok(new { Message = "Welcome to the Admin Dashboard" });
        }

        [Authorize(Roles = "TravelAgent")]
        [HttpGet("travel-agent-dashboard")]
        public IActionResult TravelAgentDashboard()
        {
            return Ok(new { Message = "Welcome to the Travel Agent Dashboard" });
        }

        private string GenerateJwtToken(UsersClass user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

