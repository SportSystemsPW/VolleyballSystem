using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Infrastructure.Database.Models
{
    public class TrainingTrainingParticipant
    {
        public int TrainingId { get; set; }
        public int TrainingParticipantId { get; set; }
        virtual public TrainingParticipant TrainingParticipant { get; set; } = null!;
        virtual public Training Training { get; set; } = null!;
        public bool Presence { get; set; }
        public bool MessageSent { get; set; }
    }
}
