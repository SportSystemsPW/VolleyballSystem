using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Infrastructure.Database.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public DateTime CreationDate { get; set; }
        public double Price { get; set; }
        public string Location { get; set; }
        public int TrainerId { get; set; }
        virtual public User Trainer { get; set; } = null!;
        virtual public ICollection<TrainingTrainingParticipant> TrainingParticipants { get; set; } = new List<TrainingTrainingParticipant>();
    }
}
