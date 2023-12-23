using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.DTO.TrainingOrganizer
{
    public class TrainingPresencesDTO
    {
        public int TrainingId { get; set; }
        public List<TrainingTrainingParticipantDTO> ParticipantDTOs { get; set; } = new List<TrainingTrainingParticipantDTO>();
    }
}
