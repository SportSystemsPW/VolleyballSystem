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
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public virtual ICollection<TrainingGroup> TrainingGroups { get; set; } = new List<TrainingGroup>();
        public virtual ICollection<TrainingParticipant> TrainingParticipants { get; set; } = new List<TrainingParticipant>();
        public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();
        public virtual ICollection<MessageTemplate> MessageTemplates { get; set; } = new List<MessageTemplate>();
    }
}
