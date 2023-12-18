using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Infrastructure.Database.Models
{
    public class TrainingParticipant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public double Balance { get; set; }
        public int TrainerId { get; set; }
        virtual public User Trainer { get; set; } = null!;
        virtual public ICollection<TrainingGroupTrainingParticipant> TrainingGroupTrainingParticipants { get; set; } = new List<TrainingGroupTrainingParticipant>();
    }
}
