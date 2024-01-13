using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Infrastructure.Database.Models
{
    public class TrainingGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TrainerId { get; set; }
        virtual public Trainer Trainer { get; set; } = null!;
        virtual public ICollection<TrainingGroupTrainingParticipant> TrainingGroupTrainingParticipants { get; set; } = new List<TrainingGroupTrainingParticipant>();
    }
}
