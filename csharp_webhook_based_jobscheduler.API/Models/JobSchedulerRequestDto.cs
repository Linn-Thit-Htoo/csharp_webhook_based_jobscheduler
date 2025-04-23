namespace csharp_webhook_based_jobscheduler.API.Models
{
    public class JobSchedulerRequestDto
    {
        public Uri Uri { get; set; }
        public string Endpoint { get; set; }
        public string? JsonPayload { get; set; }
        public CancellationToken Cs { get; set; } = default;
    }
}
