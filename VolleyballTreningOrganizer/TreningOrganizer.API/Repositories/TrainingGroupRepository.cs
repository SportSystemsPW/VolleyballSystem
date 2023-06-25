using Microsoft.EntityFrameworkCore;
using Volleyball.DTO.TrainingOrganizer;
using TreningOrganizer.API.IRepositories;
using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.Repositories
{
    public class TrainingGroupRepository : ITrainingGroupRepository
    {
        private DbSet<TrainingGroup> trainingGroups;
        private readonly VolleyballContext context;
        public TrainingGroupRepository(VolleyballContext context)
        {
            this.context = context;
            trainingGroups = context.TrainingGroups;
        }
        public void DeleteTrainingGroupById(int id)
        {
            TrainingGroup trainingGroup = trainingGroups.FirstOrDefault(tg => tg.Id == id);
            if(trainingGroup != null)
            {
                trainingGroups.Remove(trainingGroup);
                context.SaveChanges();
            }
        }

        public TrainingGroup GetTrainingGroupById(int id)
        {
            return trainingGroups.Include(tg => tg.TrainingGroupTrainingParticipants).ThenInclude(tgtp => tgtp.TrainingParticipant).FirstOrDefault(tg => tg.Id == id);
        }

        public List<TrainingGroupDTO> GetTrainingGroupsForTrainer(int trainerId)
        {
            return trainingGroups.Where(tg => tg.TrainerId == trainerId).Select(tg => new TrainingGroupDTO
            {
                Id = tg.Id,
                Name = tg.Name,
                TrainingParticipantDTOs = tg.TrainingGroupTrainingParticipants.Select(tgtp => new TrainingParticipantDTO
                {
                    Id = tgtp.TrainingParticipant.Id,
                    Name = tgtp.TrainingParticipant.Name,
                    Phone = tgtp.TrainingParticipant.Phone
                }).ToList()
            }).ToList();
        }

        public void InsertTrainingGroup(TrainingGroup group)
        {
            trainingGroups.Add(group);
            context.SaveChanges();
        }

        public void UpdateTrainingGroup(TrainingGroup group)
        {
            trainingGroups.Update(group);
            context.SaveChanges();
        }
    }
}
