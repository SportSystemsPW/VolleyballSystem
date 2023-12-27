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
        public void DeleteTrainingGroupById(int id, int trainerId)
        {
            TrainingGroup trainingGroup = trainingGroups.Include(tg => tg.TrainingGroupTrainingParticipants).FirstOrDefault(tg => tg.Id == id);
            if(trainingGroup != null)
            {
                if (trainingGroup.TrainerId != trainerId)
                    throw new TrainerNotAuthorizedException(MessageRepository.CannotRemoveObject("group"));
                context.TrainingGroupTrainingParticipants.RemoveRange(trainingGroup.TrainingGroupTrainingParticipants);
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
                MembersCount = tg.TrainingGroupTrainingParticipants.Count
            }).ToList();
        }

        public int InsertTrainingGroup(TrainingGroup group)
        {
            trainingGroups.Add(group);
            context.SaveChanges();
            return group.Id;
        }

        public void UpdateTrainingGroup(TrainingGroup group)
        {
            trainingGroups.Update(group);
            context.SaveChanges();
        }

        public Dictionary<string, int> GetTrainingGroupDictionary(int trainerId)
        {
            return trainingGroups.Where(mt => mt.TrainerId == trainerId).ToDictionary(mt => mt.Name, mt => mt.Id);
        }
    }
}
