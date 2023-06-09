using Volleyball.Infrastructure.Database.Models;

namespace TreningOrganizer.API.DTOs
{
    public class TrainingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public List<TrainingTrainingParticipantDTO> participantDTOs { get; set; } = new List<TrainingTrainingParticipantDTO>();
    }

    public class TrainingTrainingParticipantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool Presence { get; set; }
        public bool MessageSent { get; set; }
        public string Phone { get; set; } = string.Empty;
    }
}
