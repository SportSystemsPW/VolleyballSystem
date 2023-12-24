using Volleyball.DTO.TrainingOrganizer;

namespace TreningOrganizer.API.IServices
{
    public interface ITrainingService
    {
        public List<TrainingDTO> GetTrainingsForTrainer(int trainerId);
        public TrainingDTO GetTrainingById(int id);
        public int InsertTraining(TrainingDTO trainingDTO, int trainerId);
        //public void UpdateTraining(TrainingDTO trainingDTO);
        //public void DeleteTrainingById(int id);
        public void SetParticipantPresence(TrainingPresencesDTO trainingPresencesDTO);
        public List<AttendanceChangedResponseDTO> ProcessSMSResponses(List<SMSResponseDTO> smsResponseDTOs, int trainerId);
    }
}
