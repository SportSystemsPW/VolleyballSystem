using System.Text.Json.Serialization;

namespace Arbiter.API.Data
{
    public class KeyPhraseDictionary
    {
        public List<string> HOME_TEAM { get; set; } = new();

        public List<string> GUEST_TEAM { get; set; } = new();

        public List<string> POINT { get; set; } = new();
    }
}
