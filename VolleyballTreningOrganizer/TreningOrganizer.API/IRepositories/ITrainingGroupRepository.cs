using TreningOrganizer.API.DTOs;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IRepositories
{
    public interface ITrainingGroupRepository
    {
        public List<TrainingGroupDTO> GetTrainingGroupsForTrainer(int trainerId);
        public TrainingGroup GetTrainingGroupById(int id);
        public void InsertTrainingGroup(TrainingGroup group);
        public void UpdateTrainingGroup(TrainingGroup group);
        public void DeleteTrainingGroupById(int id);
    }
}
