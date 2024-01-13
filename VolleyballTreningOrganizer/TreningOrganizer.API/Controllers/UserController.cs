using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TreningOrganizer.API.IServices;
using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        private readonly IConfiguration config;

        public UserController(IConfiguration config, IUserService userService)
        {
            this.config = config;
            this.userService = userService;
        }

        [HttpPost("register")]
        public TrainingOrganizerResponse<bool> Register(TrainerDTO trainerDTO)
        {
            bool success = true;
            List<string> errors = new List<string>();
            var result = userService.Register(trainerDTO);
            if(result != string.Empty)
                errors.Add(result);

            return CreateResponse(success, errors);
        }

        [HttpPost("login")]
        public TrainingOrganizerResponse<string> Login(TrainerDTO trainerDTO)
        {
            List<string> errors = new List<string>();
            var userId = userService.Login(trainerDTO);

            if (userId == -1)
            {
                errors.Add(MessageRepository.IncorrectEmailOrPassword);
                return CreateResponse(string.Empty, errors);
            }   

            var key = Encoding.UTF8.GetBytes(config.GetValue<string>("Jwt:Key"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(240),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.OutboundClaimTypeMap.Clear();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var stringToken = tokenHandler.WriteToken(token);

            return CreateResponse(stringToken);
        }
    }
}
