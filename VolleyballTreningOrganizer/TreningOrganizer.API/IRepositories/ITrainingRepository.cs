using Volleyball.DTO.TrainingOrganizer;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IRepositories
{
    public interface ITrainingRepository
    {
        public List<TrainingDTO> GetTrainingDTOsForTrainer(int trainerId);
        public Training GetTrainingById(int id);
        public List<Training> GetTrainingsForTrainer(int trainerId);
        public int InsertTraining(Training training);
        public void UpdateTraining(Training training);
        public void DeleteTrainingById(int id);
        public TrainingTrainingParticipant GetTrainingParticipant(int participantId, int trainingId);
        public void SaveChanges();
    }
}
