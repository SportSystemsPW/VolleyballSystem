namespace Volleyball.DTO.TrainingOrganizer
{
    public class TrainingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Location { get; set; }
        public List<TrainingTrainingParticipantDTO> ParticipantDTOs { get; set; } = new List<TrainingTrainingParticipantDTO>();
    }

    public class TrainingTrainingParticipantDTO : TrainingParticipantDTO
    {
        public bool Presence { get; set; }
    }
}
