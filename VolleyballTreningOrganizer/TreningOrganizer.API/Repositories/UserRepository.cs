using Microsoft.EntityFrameworkCore;
using TreningOrganizer.API.IRepositories;
using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<Trainer> trainers;
        private readonly VolleyballContext context;
        public UserRepository(VolleyballContext context)
        {
            this.context = context;
            trainers = context.Trainers;
        }
        public Trainer GetTrainerByEmail(string email)
        {
            return trainers.FirstOrDefault(t => t.Email == email);
        }

        public void InsertTrainer(Trainer trainer)
        {
            trainers.Add(trainer);
            context.SaveChanges();
        }
    }
}
