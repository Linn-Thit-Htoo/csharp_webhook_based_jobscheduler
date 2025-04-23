using csharp_webhook_based_jobscheduler.API.Enums;
using csharp_webhook_based_jobscheduler.API.Utils;
using MediatR;

namespace csharp_webhook_based_jobscheduler.API.Features.JobScheduler.CreateJob
{
    public class CreateJobCommand : IRequest<Result<CreateJobResponse>>
    {
        public EnumJobType JobType { get; set; }
        public string Uri { get; set; }
        public string Endpoint { get; set; }
        public string? JsonPayload { get; set; }
        public DateTime? DelayAt { get; set; }
        public string? CronExpression { get; set; }
        public string? JobId { get; set; }
    }
}
