namespace csharp_webhook_based_jobscheduler.API.Features.JobScheduler.Core;

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

    [HttpDelete("CancelDelayJob")]
    public IActionResult CancelJob(string jobId)
    {
        BackgroundJob.Delete(jobId);
        return Content(Result<object>.Success());
    }

    [HttpDelete("CancelRecurJob")]
    public IActionResult CancelRecurJob(string jobId)
    {
        RecurringJob.RemoveIfExists(jobId);
        return Content(Result<object>.Success());
    }
}
