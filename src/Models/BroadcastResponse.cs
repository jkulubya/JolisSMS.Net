namespace JolisSMS.Net.Models
{
    public class BroadcastResponse
    {
        public bool Scheduled { get; set; }
        public Response Response { get; set; }
        public string Error { get; set; }
    }

    public class Response
    {
        public int ScheduleId { get; set; }
        public string Text { get; set; }
    }
}