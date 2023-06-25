using Microsoft.EntityFrameworkCore;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IRepositories;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Repositories
{
    public class TrainingParticipantRepository : ITrainingParticipantRepository
    {
        private DbSet<TrainingParticipant> trainingParticipants;
        private readonly VolleyballContext context;
        public TrainingParticipantRepository(VolleyballContext context)
        {
            this.context = context;
            trainingParticipants = context.TrainingParticipants;
        }
        public void DeleteTrainingParticipantById(int id)
        {
            TrainingParticipant trainingParticipant = trainingParticipants.FirstOrDefault(tp => tp.Id == id);
            if(trainingParticipant != null)
            {
                trainingParticipants.Remove(trainingParticipant);
                context.SaveChanges();
            }
        }

        public TrainingParticipant GetTrainingParticipantById(int id)
        {
            return trainingParticipants.FirstOrDefault(tp => tp.Id == id);
        }

        public TrainingParticipant GetTrainingParticipantByPhone(string phone, int trainerId)
        {
            return trainingParticipants.FirstOrDefault(tp => tp.Phone == phone && tp.TrainerId == trainerId);
        }

        public List<TrainingParticipantDTO> GetTrainingParticipantsForTrainer(int trainerId)
        {
            return trainingParticipants.Where(tp => tp.TrainerId == trainerId).Select(tp => new TrainingParticipantDTO
            {
                Id = tp.Id,
                Name = tp.Name,
                Phone = tp.Phone,
                Balance = tp.Balance
            }).ToList();
        }

        public void InsertTrainingParticipant(TrainingParticipant trainingParticipant)
        {
            trainingParticipants.Add(trainingParticipant);
            context.SaveChanges();
        }

        public void UpdateTrainingParticipant(TrainingParticipant trainingParticipant)
        {
            trainingParticipants.Update(trainingParticipant);
            context.SaveChanges();
        }
    }
}
