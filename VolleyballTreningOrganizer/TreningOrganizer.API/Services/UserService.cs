using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public int Login(TrainerDTO trainerDTO)
        {
            var trainer = userRepository.GetTrainerByEmail(trainerDTO.Email);
            if(trainer == null)
                return -1;
            if(trainer.PasswordHash != sha512(trainerDTO.Password))
                return -1;
            return trainer.Id;
        }

        public string Register(TrainerDTO trainerDTO)
        {
            var trainer = userRepository.GetTrainerByEmail(trainerDTO.Email);
            if (trainer != null)
            {
                return MessageRepository.UserAlreadyExists;
            }
            Trainer newUser = new Trainer()
            {
                Email = trainerDTO.Email,
                PasswordHash = sha512(trainerDTO.Password)
            };
            userRepository.InsertTrainer(newUser);
            return string.Empty;
        }

        private static string sha512(string inputString)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
