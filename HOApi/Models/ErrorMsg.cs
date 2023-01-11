using Newtonsoft.Json;

namespace HOApi.Models
{
    public class ErrorMsg
    {
        [JsonProperty]
        string reason { get; }
        [JsonProperty]
        string error { get; }
        [JsonProperty]
        string message { get; }

        public ErrorMsg(string message,string error,string reason="")
        {
            this.message = message;
            this.error = error;
            this.reason = reason;
        }
    }
}
