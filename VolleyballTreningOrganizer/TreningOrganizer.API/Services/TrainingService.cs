using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IRepositories;
using TreningOrganizer.API.IServices;
using TreningOrganizer.API.Repositories;
using Volleyball.Infrastructure.Database.Models;
using Azure;
using System.Text.RegularExpressions;

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
        //public void DeleteTrainingById(int id)
        //{
        //    trainingRepository.DeleteTrainingById(id);
        //}

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
                Location = training.Location,
                ParticipantDTOs = training.TrainingParticipants.Select(tp => new TrainingTrainingParticipantDTO
                {
                    Id = tp.TrainingParticipantId,
                    Name = tp.TrainingParticipant.Name,
                    Phone = tp.TrainingParticipant.Phone,
                    Presence = tp.Presence
                }).ToList()
            };
        }

        public List<TrainingDTO> GetTrainingsForTrainer(int trainerId)
        {
            return trainingRepository.GetTrainingDTOsForTrainer(trainerId);
        }

        public int InsertTraining(TrainingDTO trainingDTO, int trainerId)
        {
            Training training = new Training
            {
                Date = trainingDTO.Date,
                Name = trainingDTO.Name,
                Message = trainingDTO.Name,
                Price = trainingDTO.Price,
                Location = trainingDTO.Location,
                TrainerId = trainerId,
                CreationDate = DateTime.Now
            };
            List<TrainingTrainingParticipant> trainingParticipants = new List<TrainingTrainingParticipant>();
            foreach (TrainingTrainingParticipantDTO tp in trainingDTO.ParticipantDTOs)
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
            return trainingRepository.InsertTraining(training);
        }

        //public void UpdateTraining(TrainingDTO trainingDTO)
        //{
        //    Training training = trainingRepository.GetTrainingById(trainingDTO.Id);
        //    if(training == null)
        //    {
        //        throw new Exception();
        //    }

        //    int trainerId = training.TrainerId;
        //    double oldPrice = training.Price;

        //    training.Price = trainingDTO.Price;
        //    training.Name = trainingDTO.Name;
        //    training.Date = trainingDTO.Date;
        //    training.Message = trainingDTO.Message;

        //    //TODO zmiana uczestników treningu
        //    foreach(var tp in training.TrainingParticipants)
        //    {
        //        if (tp.Presence) tp.TrainingParticipant.Balance += oldPrice - training.Price;
        //    }
        //    trainingRepository.UpdateTraining(training);
        //}

        public void SetParticipantPresence(TrainingPresencesDTO trainingPresencesDTO)
        {
            int trainingId = trainingPresencesDTO.TrainingId;
            Training training = trainingRepository.GetTrainingById(trainingId);
            foreach(var participant in trainingPresencesDTO.ParticipantDTOs)
            {
                TrainingTrainingParticipant tp = trainingRepository.GetTrainingParticipant(participant.Id, trainingId);
                if (tp != null)
                {
                    if (participant.Presence != tp.Presence)
                    {
                        if (participant.Presence)
                        {
                            tp.TrainingParticipant.Balance -= training.Price;
                        }
                        else
                        {
                            tp.TrainingParticipant.Balance += training.Price;
                        }
                    }
                    tp.Presence = participant.Presence;
                    trainingRepository.SaveChanges();
                }
            }
        }

        public List<AttendanceChangedResponseDTO> ProcessSMSResponses(List<SMSResponseDTO> smsResponseDTOs, int trainerId)
        {
            Dictionary<int, int> trainingAttendancesChanged = new Dictionary<int, int>();
            List<AttendanceChangedResponseDTO> result = new List<AttendanceChangedResponseDTO>();
            List<Training> trainings = trainingRepository.GetTrainingsForTrainer(trainerId);
            smsResponseDTOs = smsResponseDTOs.OrderByDescending(r => r.DateTime).ToList();

            foreach (Training training in trainings)
            {
                for(int i = 0; i < smsResponseDTOs.Count; i++)
                {
                    if (smsResponseDTOs.ElementAt(i).DateTime > training.CreationDate)
                    {
                        var participant = training.TrainingParticipants.FirstOrDefault(p => p.TrainingParticipant.Phone.ComparePhoneNumber(smsResponseDTOs.ElementAt(i).Phone));
                        if (participant != null && !participant.Presence)
                        {
                            participant.TrainingParticipant.Balance -= training.Price;
                            participant.Presence = true;
                            if (trainingAttendancesChanged.ContainsKey(training.Id))
                            {
                                trainingAttendancesChanged[training.Id] += 1;
                            }
                            else
                            {
                                trainingAttendancesChanged[training.Id] = 1;
                            }
                            smsResponseDTOs.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            trainingRepository.SaveChanges();

            foreach(var entry in trainingAttendancesChanged)
            {
                result.Add(new AttendanceChangedResponseDTO
                {
                    TrainingId = entry.Key,
                    AttendanceCountDelta = entry.Value
                });
            }

            return result;
        }
    }

    public static class PhoneNumberComparer
    {
        //numbers read from contacts can have whitespcaes or some other divider used eg. 123-123-123
        //numbers read from sms have different formating, so compare only digits
        public static bool ComparePhoneNumber(this string phoneNumberA, string phoneNumberB)
        {
            return Regex.Replace(phoneNumberA, @"[^\d]", "") == Regex.Replace(phoneNumberB, @"[^\d]", "");
        }
    }
}
