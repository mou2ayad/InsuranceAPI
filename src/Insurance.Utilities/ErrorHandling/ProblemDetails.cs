using Newtonsoft.Json;

namespace Insurance.Utilities.ErrorHandling
{
    public class ProblemDetails
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("traceId")]
        public string TraceId { get; set; }
        [JsonProperty("details")]
        public string Details { get; set; }

    }
}