using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IRepositories
{
    public interface IUserRepository
    {
        Trainer GetTrainerByEmail(string email);
        void InsertTrainer(Trainer trainer);
    }
}
