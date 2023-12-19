namespace Volleyball.DTO.TrainingOrganizer
{
    public class TrainingGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MembersCount { get; set; }
        public List<TrainingParticipantDTO> TrainingParticipantDTOs { get; set; } = new List<TrainingParticipantDTO>();
    }
}
