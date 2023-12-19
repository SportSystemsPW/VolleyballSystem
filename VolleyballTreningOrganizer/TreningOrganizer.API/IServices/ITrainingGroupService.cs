using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IServices
{
    public interface ITrainingGroupService
    {
        public List<TrainingGroupDTO> GetTrainingGroupsForTrainer(int trainerId);
        public TrainingGroupDTO GetTrainingGroupById(int id);
        public int InsertTrainingGroup(TrainingGroupDTO groupDTO, int trainerId);
        public void UpdateTrainingGroup(TrainingGroupDTO groupDTO);
        public void DeleteTrainingGroupById(int id);
    }
}
