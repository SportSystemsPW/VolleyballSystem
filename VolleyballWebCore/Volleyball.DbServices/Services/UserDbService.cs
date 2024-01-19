using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Discussion;
using Volleyball.DTO.Teams;
using Volleyball.DTO.Users;
using Volleyball.Infrastructure.Database.Models;

namespace VolleyballDomain.Shared.Services
{
    public class UserDbService
    {
        private readonly VolleyballContext _context;
        private readonly PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
        public UserDbService()
        {
            _context = new VolleyballContext();

        }

        // Service to register a new user
        public async Task<ServiceResponse> RegisterAsync(RegisterDto registerDto)
        {
            var response = new ServiceResponse();

            var existingUser = _context.Users.Include(u => u.Credentials).FirstOrDefault(u => u.Credentials != null && u.Credentials.Email == registerDto.Email);

            if (existingUser != null)
            {
                response.Success = false;
                response.Message = "Adres e-mail jest już w użyciu.";
                return response;
            }

            var user = _context.Users.Include(u => u.Credentials).FirstOrDefault(u => u.AdditionalEmail == registerDto.Email);

            if (user != null && user.Credentials == null)
            {
                // someone has added this email to a team
                user.FirstName = registerDto.FirstName;
                user.LastName = registerDto.LastName;
                user.Phone = registerDto.PhoneNumber;
                user.PositionId = registerDto.PositionId;
                user.BirthYear = registerDto.BirthYear;
                user.Height = (byte?)registerDto.Height;
                user.Weight = (byte?)registerDto.Weight;
                user.JerseyNumber = (byte?)registerDto.JerseyNumber;
                user.AttackRange = (byte?)registerDto.AttackRange;
                user.BlockRange = (byte?)registerDto.BlockRange;
                user.VolleyballIdol = registerDto.VolleyballIdol;
                user.AdditionalEmail = registerDto.AdditionalEmail;
                user.PersonalInfo = registerDto.PersonalInfo;
                user.City = registerDto.City;
                user.Hobby = registerDto.Hobby;
            }
            else
            {
                user = ConvertToUser(registerDto);
                _context.Users.Add(user);
            }


            var playerRole = _context.Roles.First(r => r.Name == Roles.Player);

            var credentials = new Credentials
            {
                Email = registerDto.Email,
                Password = HashPassword(registerDto.Email, registerDto.Password), // Hash the password
                User = user,
                Roles = new List<Role> { playerRole }

            };

            _context.Credentials.Add(credentials);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.ToString();
                //response.Message = "Błąd zapisu danych. Spróbuj ponownie później";
            }
            return response;
        }

        // Service to login a user
        public ServiceResponse Login(LoginDto loginDto, out Credentials? credentials)
        {
            var response = new ServiceResponse<List<Role>>();
            credentials = _context.Credentials.Include(c => c.Roles).FirstOrDefault(c => c.Email == loginDto.Login);


            if (credentials == null || !VerifyPassword(loginDto.Login, loginDto.Password, credentials.Password))
            {
                response.Success = false;
                response.Message = "Błędny e-mail lub hasło";
                return response;
            }

            response.Data = credentials.Roles.ToList();

            return response;
        }



        // Service to hash and update password
        public async Task<ServiceResponse> UpdatePasswordAsync(string userId, UpdatePasswordDto updatePasswordDto)
        {
            var response = new ServiceResponse();
            var credentials = await _context.Credentials.Include(c => c.User).FirstOrDefaultAsync(c => c.Email == userId);

            if (credentials == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";
                return response;
            }

            var user = credentials.User;

            string hashedPassword = HashPassword(credentials.Email, updatePasswordDto.NewPassword);
            credentials.Password = hashedPassword;
            await _context.SaveChangesAsync();

            return response;
        }

        // Service to update user
        public async Task<ServiceResponse> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            var response = new ServiceResponse();
            var credentials = await _context.Credentials.Include(c => c.User).FirstOrDefaultAsync(c => c.Email == userId);

            if (credentials == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";
                return response;
            }

            var user = credentials.User;

            user.AttackRange = updateUserDto.AttackRange;
            user.BirthYear = updateUserDto.BirthYear;
            user.City = updateUserDto.City;
            user.BlockRange = updateUserDto.BlockRange;
            user.FirstName = updateUserDto.FirstName ?? user.FirstName;
            user.Gender = updateUserDto.Gender;
            user.Height = (byte?)(updateUserDto.Height);
            user.Weight = (byte?)(updateUserDto.Weight);
            user.JerseyNumber = (byte?)(updateUserDto.JerseyNumber);
            user.VolleyballIdol = updateUserDto.VolleyballIdol;
            user.Hobby = updateUserDto.Hobby;
            user.Photo = updateUserDto.Photo;
            user.LastName = updateUserDto.LastName ?? user.LastName;
            user.PositionId = updateUserDto.PositionId;
            user.PersonalInfo = updateUserDto.PersonalInfo;

            await _context.SaveChangesAsync();

            return response;
        }

        // Service to get player summary
        public async Task<ServiceResponse<PlayerSummaryDto>> GetPlayerSummary(string email)
        {
            var response = new ServiceResponse<PlayerSummaryDto>();

            var credentials = await _context.Credentials.Include(c => c.User).FirstOrDefaultAsync(c => c.Email == email);

            if (credentials == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";
                return response;
            }

            var player = new PlayerSummaryDto()
            {
                Id = credentials.User.Id,
                FirstName = credentials.User.FirstName,
                LastName = credentials.User.LastName,
                Photo = credentials.User.Photo,
            };

            response.Data = player;

            return response;
        }

        // Service to get user profile
        public async Task<ServiceResponse<UserProfileDto>> GetUserProfileAsync(int id)
        {
            var response = new ServiceResponse<UserProfileDto>();

            var user = await _context.Users.Include(u => u.Position).FirstOrDefaultAsync(u => u.Id == id);

            var comments = await _context.Comments.Where(c =>
                c.ContentId == id &&
                c.CommentLocationId == (int)CommentLocations.Player
            ).ToListAsync();

            if (user == null)
            {
                response.Success = false;
                response.Message = "Nie znaleziono użytkownika";
                return response;
            }

            var player = (UserProfileDto)user;

            response.Data = player;

            return response;
        }

        // Get user profile by email
        public async Task<ServiceResponse<UserProfileDto>> GetUserProfileByEmailAsync(string email)
        {
            int userId = (await _context.Credentials.FirstOrDefaultAsync(c => c.Email == email))?.UserId ?? 0;
            if (userId == 0)
            {
                return new ServiceResponse<UserProfileDto>()
                {
                    Success = false,
                    Message = "Nie znaleziono użytkownika"
                };
            }
            return await GetUserProfileAsync(userId);
        }


        // check if player is a captain of any team
        public async Task<ServiceResponse<bool>> IsTeamCaptain(string playerEmail)
        {
            var response = new ServiceResponse<bool>();
            var user = (await _context.Credentials.Include(c => c.User).ThenInclude(u => u.Team).FirstOrDefaultAsync(p => p.Email == playerEmail))?.User;
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            response.Data = user.Team != null;

            return response;
        }


        // Password methods

        private bool VerifyPassword(string email, string password, string hashedPassword)
        {
            string hash = PepperPassowrd(password);
            var result = passwordHasher.VerifyHashedPassword(email, hashedPassword, hash);
            return result == PasswordVerificationResult.Success;
        }
        private string HashPassword(string email, string password)
        {
            string hash = PepperPassowrd(password);

            hash = passwordHasher.HashPassword(email, hash);
            return hash;

        }

        private string PepperPassowrd(string password)
        {
            string pepper = "qaSJKvYBm9$$;=EDOC-)EJ0m";
            return password + pepper;
        }

        // Helper methods
        private User ConvertToUser(RegisterDto registerDto)
        {
            return new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                AccountId = null,
                BirthYear = registerDto.BirthYear,
                City = registerDto.City,
                PersonalInfo = registerDto.PersonalInfo,
                Photo = null,
                Gender = registerDto.Gender,
                Height = (byte?)registerDto.Height,
                Weight = (byte?)registerDto.Weight,
                JerseyNumber = (byte?)registerDto.JerseyNumber,
                BlockRange = registerDto.BlockRange,
                AttackRange = registerDto.AttackRange,
                VolleyballIdol = registerDto.VolleyballIdol,
                AdditionalEmail = registerDto.AdditionalEmail,
                Hobby = registerDto.Hobby,
                Phone = null,
                PositionId = registerDto.PositionId > 0 ? registerDto.PositionId : 1,
                PhotoWidth = null,
                PhotoHeight = null,
                Articles = new List<Article>(),
                Team = null,
                TeamPlayers = new List<TeamPlayer>(), // Initialize as needed
            };
        }

        public class NotFoundException : Exception { }
    }
}
