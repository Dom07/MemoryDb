using Newtonsoft.Json;

namespace MemoryDb.Console.Models
{
    public class Command
    {
        [JsonProperty("command")]
        public Operation Operation { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; } = string.Empty;
        [JsonProperty("value")]
        public object Value { get; set; } = new();
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; } = string.Empty;
    }
}
