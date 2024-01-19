using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DbServices.Services;
using Volleyball.DTO.Users;
using Volleyball.Infrastructure.Database.Models;
using VolleyballDomain.Shared;
using VolleyballDomain.Shared.Services;

namespace Volleyball.Api.Controllers
{
    [ApiController]
    //[EnableCors]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbService userDbService = new UserDbService();

        private readonly IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [Route("usersummary")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserSummary()
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var result = await userDbService.GetPlayerSummary(id);
            return Ok(result);
        }

        // Register a new user
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await userDbService.RegisterAsync(registerDto);

            return Ok(result);
        }

        // Log in
        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {

            var serviceResult = userDbService.Login(loginDto, out Credentials? credentials);

            var response = new ServiceResponse<string>() { Data = "", Message = serviceResult.Message, Success = serviceResult.Success };

            if (!serviceResult.Success || credentials == null)
            {
                return Ok(response);
            }

            string issuer = _config.GetValue<string>("Jwt:Issuer");
            string audience = _config.GetValue<string>("Jwt:Audience");
            var key = Encoding.UTF8.GetBytes(_config.GetValue<string>("Jwt:Key"));

            List<Claim> roles = new List<Claim>();

            foreach (var role in credentials.Roles)
            {
                roles.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
        {
                        new Claim(ClaimTypes.Name, credentials.Email),
                        new Claim(ClaimTypes.NameIdentifier, credentials.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(240),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            tokenDescriptor.Subject.AddClaims(roles);

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.OutboundClaimTypeMap.Clear();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var stringToken = tokenHandler.WriteToken(token);

            response.Data = stringToken;

            return Ok(response);

        }

        // Update password
        [Authorize]
        [Route("updatepassword")]
        [HttpPut]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            string? id = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var result = await userDbService.UpdatePasswordAsync(id, updatePasswordDto);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Update user data
        [Authorize]
        [Route("updateuserdata")]
        [HttpPut]
        public async Task<IActionResult> UpdateUserData(UpdateUserDto updateUserDto)
        {
            string? id = User.Identity?.Name;

            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var result = await userDbService.UpdateUserAsync(id, updateUserDto);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Get user profile by id
        [Route("userprofile/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var result = await userDbService.GetUserProfileAsync(id);

            return Ok(result);

        }

        // get current user profile
        [Route("myprofile")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var result = await userDbService.GetUserProfileByEmailAsync(id);
            return Ok(result);
        }



        [HttpGet]
        [Route("isteamcaptain")]
        [Authorize]
        public async Task<IActionResult> IsTeamCaptain()
        {
            string? id = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var result = await userDbService.IsTeamCaptain(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}