namespace Arbiter.API.DTOs
{
    public class MatchReportDto
    {
        public int MatchId { get; set; }
        public string Action { get; set; } = null!;
        public string ArbiterSentence { get; set; } = null!;
        public int Set { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
