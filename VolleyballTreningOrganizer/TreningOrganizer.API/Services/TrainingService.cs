using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Repositories;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository trainingRepository;
        private readonly ITrainingParticipantRepository trainingParticipantRepository;
        public TrainingService(ITrainingRepository trainingRepository, ITrainingParticipantRepository trainingParticipantRepository)
        {
            this.trainingRepository = trainingRepository;
            this.trainingParticipantRepository = trainingParticipantRepository;

        }
        public void DeleteTrainingById(int id)
        {
            trainingRepository.DeleteTrainingById(id);
        }

        public TrainingDTO GetTrainingById(int id)
        {
            Training training = trainingRepository.GetTrainingById(id);
            return new TrainingDTO
            {
                Id = training.Id,
                Name = training.Name,
                Date = training.Date,
                Message = training.Message,
                Price = training.Price,
                participantDTOs = training.TrainingParticipants.Select(tp => new TrainingTrainingParticipantDTO
                {
                    Id = tp.TrainingParticipantId,
                    Name = tp.TrainingParticipant.Name,
                    Presence = tp.Presence,
                    MessageSent = tp.MessageSent
                }).ToList()
            };
        }

        public List<TrainingDTO> GetTrainingsForTrainer(int trainerId)
        {
            return trainingRepository.GetTrainingsForTrainer(trainerId);
        }

        public void InsertTraining(TrainingDTO trainingDTO, int trainerId)
        {
            Training training = new Training
            {
                Date = trainingDTO.Date,
                Name = trainingDTO.Name,
                Message = trainingDTO.Name,
                Price = trainingDTO.Price,
                TrainerId = trainerId
            };
            List<TrainingTrainingParticipant> trainingParticipants = new List<TrainingTrainingParticipant>();
            foreach (TrainingTrainingParticipantDTO tp in trainingDTO.participantDTOs)
            {
                TrainingParticipant participant = trainingParticipantRepository.GetTrainingParticipantByPhone(tp.Phone, trainerId);
                if (participant == null)
                {
                    participant = new TrainingParticipant
                    {
                        Name = tp.Name,
                        Phone = tp.Phone,
                        TrainerId = trainerId,
                        Balance = 0
                    };
                }
                trainingParticipants.Add(new TrainingTrainingParticipant
                {
                    TrainingParticipant = participant
                });
            }
            training.TrainingParticipants = trainingParticipants;
            trainingRepository.InsertTraining(training);
        }

        public void UpdateTraining(TrainingDTO trainingDTO)
        {
            Training training = trainingRepository.GetTrainingById(trainingDTO.Id);
            if(training == null)
            {
                throw new Exception();
            }

            int trainerId = training.TrainerId;
            double oldPrice = training.Price;

            training.Price = trainingDTO.Price;
            training.Name = trainingDTO.Name;
            training.Date = trainingDTO.Date;
            training.Message = trainingDTO.Message;

            //TODO zmiana uczestników treningu
            foreach(var tp in training.TrainingParticipants)
            {
                if (tp.Presence) tp.TrainingParticipant.Balance += oldPrice - training.Price;
            }
            trainingRepository.UpdateTraining(training);
        }

        public void SetParticipantPresence(int participantId, int trainingId, bool presence)
        {
            Training training = trainingRepository.GetTrainingById(trainingId);
            TrainingTrainingParticipant tp = trainingRepository.GetTrainingParticipant(participantId, trainingId);
            if(tp != null)
            {
                if(presence != tp.Presence)
                {
                    if (presence)
                    {
                        tp.TrainingParticipant.Balance -= training.Price;
                    }
                    else
                    {
                        tp.TrainingParticipant.Balance += training.Price;
                    }
                }
                tp.Presence = presence;
                trainingRepository.SaveChanges();
            }
        }
    }
}
