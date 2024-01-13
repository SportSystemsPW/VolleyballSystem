﻿using Microsoft.EntityFrameworkCore;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IRepositories;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private DbSet<Training> trainings;
        private DbSet<TrainingTrainingParticipant> trainingTrainingParticipants;
        private readonly VolleyballContext context;
        public TrainingRepository(VolleyballContext context)
        {
            this.context = context;
            trainings = context.Trainings;
            trainingTrainingParticipants = context.TrainingTrainingParticipants;
        }
        public void DeleteTrainingById(int id)
        {
            Training training = trainings.FirstOrDefault(t => t.Id == id);
            if(training != null)
            {
                trainings.Remove(training);
                context.SaveChanges();
            }
        }

        public List<TrainingDTO> GetTrainingDTOsForTrainer(int trainerId)
        {
            return trainings.Where(t => t.TrainerId == trainerId).Select(t => new TrainingDTO
            {
                Id = t.Id,
                Date = t.Date,
                Name = t.Name,
                Price = t.Price,
                Location = t.Location,
                ParticipantDTOs = t.TrainingParticipants.Select(tp => new TrainingTrainingParticipantDTO
                {
                    Presence = tp.Presence
                }).ToList()
            }).ToList();
        }

        public Training GetTrainingById(int id)
        {
            return trainings.Include(t => t.TrainingParticipants).ThenInclude(tp => tp.TrainingParticipant).FirstOrDefault(t => t.Id == id);
        }

        public List<Training> GetTrainingsForTrainer(int trainerId)
        {
            return trainings.Include(t => t.TrainingParticipants).ThenInclude(tp => tp.TrainingParticipant).Where(t => t.TrainerId == trainerId).OrderByDescending(t => t.Date).ToList();
        }

        public int InsertTraining(Training training)
        {
            trainings.Add(training);
            context.SaveChanges();
            return training.Id;
        }

        public void UpdateTraining(Training training)
        {
            trainings.Update(training);
            context.SaveChanges();
        }
        public TrainingTrainingParticipant GetTrainingParticipant(int participantId, int trainingId)
        {
            return trainingTrainingParticipants.Include(tp => tp.TrainingParticipant).FirstOrDefault(tp => tp.TrainingId == trainingId && tp.TrainingParticipantId == participantId);
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}