using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Infrastructure.Database.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        virtual public ICollection<TrainingGroup> TrainingGroups { get; set; } = new List<TrainingGroup>();
        virtual public ICollection<TrainingParticipant> TrainingParticipants { get; set; } = new List<TrainingParticipant>();
        virtual public ICollection<Training> Trainings { get; set; } = new List<Training>();
        virtual public ICollection<MessageTemplate> MessageTemplates { get; set; } = new List<MessageTemplate>();
    }
}
