using csharp_webhook_based_jobscheduler.API.Features.Core;
using csharp_webhook_based_jobscheduler.API.Features.JobScheduler.CreateJob;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace csharp_webhook_based_jobscheduler.API.Features.JobScheduler.Core
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobSchedulerController : BaseController
    {
        private readonly ISender _sender;

        public JobSchedulerController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("ScheduleJob")]
        public async Task<IActionResult> ScheduleJob(CreateJobCommand command, CancellationToken cs)
        {
            var result = await _sender.Send(command, cs);
            return Content(result);
        }
    }
}
