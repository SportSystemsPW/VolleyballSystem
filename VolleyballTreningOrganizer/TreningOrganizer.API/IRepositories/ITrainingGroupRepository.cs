using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IRepositories
{
    public interface ITrainingGroupRepository
    {
        public List<TrainingGroupDTO> GetTrainingGroupsForTrainer(int trainerId);
        public TrainingGroup GetTrainingGroupById(int id);
        public int InsertTrainingGroup(TrainingGroup group);
        public void UpdateTrainingGroup(TrainingGroup group);
        public void DeleteTrainingGroupById(int id);
        public Dictionary<string, int> GetTrainingGroupDictionary(int trainerId);
    }
}
