using TreningOrganizer.API.DTOs;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Repositories;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Services
{
    public class TrainingGroupService : ITrainingGroupService
    {
        private readonly ITrainingGroupRepository trainingGroupRepository;
        private readonly ITrainingParticipantRepository trainingParticipantRepository;
        public TrainingGroupService(ITrainingGroupRepository trainingGroupRepository, ITrainingParticipantRepository trainingParticipantRepository)
        {
            this.trainingGroupRepository = trainingGroupRepository;
            this.trainingParticipantRepository = trainingParticipantRepository;
        }
        public void DeleteTrainingGroupById(int id)
        {
            trainingGroupRepository.DeleteTrainingGroupById(id);
        }
        public TrainingGroupDTO GetTrainingGroupById(int id)
        {
            TrainingGroup trainingGroup = trainingGroupRepository.GetTrainingGroupById(id);
            return new TrainingGroupDTO
            {
                Id = trainingGroup.Id,
                Name = trainingGroup.Name,
                TrainingParticipantDTOs = trainingGroup.TrainingGroupTrainingParticipants.Select(tgtp => new TrainingParticipantDTO
                {
                    Id = tgtp.TrainingParticipant.Id,
                    Name = tgtp.TrainingParticipant.Name,
                    Phone = tgtp.TrainingParticipant.Phone
                }).ToList()
            };
        }
        public List<TrainingGroupDTO> GetTrainingGroupsForTrainer(int trainerId)
        {
            return trainingGroupRepository.GetTrainingGroupsForTrainer(trainerId);
        }
        public void InsertTrainingGroup(TrainingGroupDTO groupDTO, int trainerId)
        {
            TrainingGroup trainingGroup = new TrainingGroup
            {
                Name = groupDTO.Name,
                TrainerId = trainerId
            };
            List<TrainingGroupTrainingParticipant> groupTrainingParticipants = new List<TrainingGroupTrainingParticipant>();
            foreach (TrainingParticipantDTO tp in groupDTO.TrainingParticipantDTOs)
            {
                TrainingParticipant participant = trainingParticipantRepository.GetTrainingParticipantByPhone(tp.Phone, trainerId);
                if (participant == null)
                {
                    participant = new TrainingParticipant
                    {
                        Name = tp.Name,
                        Phone = tp.Phone,
                        TrainerId = trainerId,
                        Balance = 0.0
                    };
                }
                groupTrainingParticipants.Add(new TrainingGroupTrainingParticipant
                {
                    TrainingParticipant = participant
                });
            }
            trainingGroup.TrainingGroupTrainingParticipants = groupTrainingParticipants;
            trainingGroupRepository.InsertTrainingGroup(trainingGroup);
        }
        public void UpdateTrainingGroup(TrainingGroupDTO groupDTO)
        {
            TrainingGroup trainingGroup = trainingGroupRepository.GetTrainingGroupById(groupDTO.Id);
            if (trainingGroup == null)
            {
                throw new Exception();
            }
            int trainerId = trainingGroup.TrainerId;
            List<TrainingGroupTrainingParticipant> groupTrainingParticipants = new List<TrainingGroupTrainingParticipant>();
            foreach (TrainingParticipantDTO tp in groupDTO.TrainingParticipantDTOs)
            {
                TrainingParticipant participant = trainingParticipantRepository.GetTrainingParticipantByPhone(tp.Phone, trainerId);
                if (participant == null)
                {
                    participant = new TrainingParticipant
                    {
                        Name = tp.Name,
                        Phone = tp.Phone,
                        TrainerId = trainerId,
                        Balance = 0.0
                    };
                }
                groupTrainingParticipants.Add(new TrainingGroupTrainingParticipant
                {
                    TrainingParticipant = participant
                });
            }
            trainingGroup.TrainingGroupTrainingParticipants = groupTrainingParticipants;
            trainingGroupRepository.InsertTrainingGroup(trainingGroup);
        }
    }
}
