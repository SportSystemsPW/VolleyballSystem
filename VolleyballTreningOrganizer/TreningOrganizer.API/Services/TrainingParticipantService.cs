using TreningOrganizer.API.DTOs;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Services
{
    public class TrainingParticipantService : ITrainingParticipantService
    {
        private readonly ITrainingParticipantRepository trainingParticipantRepository;
        public TrainingParticipantService(ITrainingParticipantRepository trainingParticipantRepository)
        {
            this.trainingParticipantRepository = trainingParticipantRepository;
        }
        public void DeleteTrainingParticipantById(int id)
        {
            trainingParticipantRepository.DeleteTrainingParticipantById(id);
        }
        public TrainingParticipantDTO GetTrainingParticipantById(int id)
        {
            TrainingParticipant trainingParticipant = trainingParticipantRepository.GetTrainingParticipantById(id);
            return new TrainingParticipantDTO
            {
                Id = trainingParticipant.Id,
                Name = trainingParticipant.Name,
                Phone = trainingParticipant.Phone,
                Balance = trainingParticipant.Balance
            };
        }

        public List<TrainingParticipantDTO> GetTrainingParticpantsForTrainer(int trainerId)
        {
            return trainingParticipantRepository.GetTrainingParticipantsForTrainer(trainerId);
        }

        public void InsertTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO, int trainerId)
        {
            TrainingParticipant trainingParticipant = new TrainingParticipant
            {
                Name = trainingParticipantDTO.Name,
                Phone = trainingParticipantDTO.Phone,
                TrainerId = trainerId,
                Balance = 0.0,
            };
            trainingParticipantRepository.InsertTrainingParticipant(trainingParticipant);
        }

        public void UpdateTrainingParticipant(TrainingParticipantDTO trainingParticipantDTO)
        {
            TrainingParticipant trainingParticipant = trainingParticipantRepository.GetTrainingParticipantById(trainingParticipantDTO.Id);
            if(trainingParticipant == null)
            {
                throw new Exception();
            }
            trainingParticipant.Name = trainingParticipantDTO.Name;
            trainingParticipant.Phone = trainingParticipantDTO.Phone;
            trainingParticipant.Balance = trainingParticipantDTO.Balance;
            trainingParticipantRepository.UpdateTrainingParticipant(trainingParticipant);
        }
    }
}
