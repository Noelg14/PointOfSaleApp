namespace HOApi.Models
{
    public class ErrorMsg
    {
        string reason { get; }
        string error { get; }
        string message { get; }

        public ErrorMsg(string message,string error,string reason="")
        {
            this.message = message;
            this.error = error;
            this.reason = reason;
        }
    }
}
