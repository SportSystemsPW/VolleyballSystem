namespace TreningOrganizer.API.DTOs
{
    public class TrainingGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<TrainingParticipantDTO> TrainingParticipantDTOs { get; set; } = new List<TrainingParticipantDTO>();
    }
}
