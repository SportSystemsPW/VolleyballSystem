using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Infrastructure.Database.Models
{
    public class TrainingGroupTrainingParticipant
    {
        public int TrainingGroupId { get; set; }
        public int TrainingParticipantId { get; set; }
        public virtual TrainingGroup TrainingGroup { get; set; } = null!;
        public virtual TrainingParticipant TrainingParticipant { get; set; } = null!;
    }
}
