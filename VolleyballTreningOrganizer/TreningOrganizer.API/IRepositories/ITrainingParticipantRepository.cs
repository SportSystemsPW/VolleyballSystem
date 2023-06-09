using TreningOrganizer.API.DTOs;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.IRepositories
{
    public interface ITrainingParticipantRepository
    {
        public List<TrainingParticipantDTO> GetTrainingParticipantsForTrainer(int trainerId);
        public TrainingParticipant GetTrainingParticipantById(int id);
        public TrainingParticipant GetTrainingParticipantByPhone(string phone, int trainerId);
        public void InsertTrainingParticipant(TrainingParticipant trainingParticipant);
        public void UpdateTrainingParticipant(TrainingParticipant trainingParticipant);
        public void DeleteTrainingParticipantById(int id);
    }
}
