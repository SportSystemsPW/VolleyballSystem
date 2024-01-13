using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.API.IServices
{
    public interface IUserService
    {
        int Login(TrainerDTO trainerDTO);
        string Register(TrainerDTO trainerDTO);
    }
}
