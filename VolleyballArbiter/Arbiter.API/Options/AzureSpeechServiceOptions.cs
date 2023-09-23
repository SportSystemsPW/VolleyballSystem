using System.ComponentModel.DataAnnotations;

namespace Arbiter.API.Options
{
    public class AzureSpeechServiceOptions
    {
        [Required]
        public string SubscriptionKey { get; set; } = string.Empty;

        [Required]
        public string ServiceRegion { get; set; } = string.Empty;
    }
}