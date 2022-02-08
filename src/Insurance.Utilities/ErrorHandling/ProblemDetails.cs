using Newtonsoft.Json;

namespace Insurance.Utilities.ErrorHandling
{
    internal class ProblemDetails
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("traceId")]
        public string TraceId { get; set; }

    }
}