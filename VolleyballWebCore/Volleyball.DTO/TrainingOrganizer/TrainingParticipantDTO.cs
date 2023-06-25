namespace Volleyball.DTO.TrainingOrganizer
{
    public class TrainingParticipantDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public double Balance { get; set; }
    }
}
