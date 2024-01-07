namespace VolleyballArbiterWeb.Models
{
    public class MatchInfoModel
    {
        public int MatchId { get; set; }
        public int? MatchStatus { get; set; }
        public int? Set1Status { get; set; }
        public int? Set2Status { get; set; }
        public int? Set3Status { get; set; }
        public int? Set4Status { get; set; }
        public int? Set5Status { get; set; }
        public string? Set1Score { get; set; }
        public string? Set2Score { get; set; }
        public string? Set3Score { get; set; }
        public string? Set4Score { get; set; }
        public string? Set5Score { get; set; }
        public string? MatchScore { get; set; }
    }
}
