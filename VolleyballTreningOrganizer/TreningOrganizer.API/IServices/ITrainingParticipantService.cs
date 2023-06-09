using TreningOrganizer.API.DTOs;

namespace TreningOrganizer.API.IServices
{
    public interface ITrainingParticipantService
    {
        public List<TrainingParticipantDTO> GetTrainingParticpantsForTrainer(int trainerId);
        public TrainingParticipantDTO GetTrainingParticipantById(int id);
        public void InsertTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO, int trainerId);
        public void UpdateTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO);
        public void DeleteTrainingParticipantById(int id);
    }
}
