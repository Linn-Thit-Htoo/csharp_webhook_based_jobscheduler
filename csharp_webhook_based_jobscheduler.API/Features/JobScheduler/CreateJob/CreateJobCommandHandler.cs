namespace csharp_webhook_based_jobscheduler.API.Features.JobScheduler.CreateJob;

public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Result<CreateJobResponse>>
{
    private readonly IValidator<CreateJobCommand> _createJobValidator;

    public CreateJobCommandHandler(IValidator<CreateJobCommand> createJobValidator)
    {
        _createJobValidator = createJobValidator;
    }

    public async Task<Result<CreateJobResponse>> Handle(
        CreateJobCommand request,
        CancellationToken cancellationToken
    )
    {
        Result<CreateJobResponse> result;

        var validationResult = await _createJobValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            result = Result<CreateJobResponse>.Fail(
                string.Join(" ", validationResult.Errors.Select(x => x.ErrorMessage))
            );
            goto result;
        }

        #region Job Type and payload validation

        #region Delay

        if (request.JobType == EnumJobType.Delay)
        {
            if (request.DelayAt is null || request.DelayAt == default)
            {
                result = Result<CreateJobResponse>.Fail("DelayAt is required for Delay job type.");
                goto result;
            }
        }
        #endregion

        #region Recur

        if (request.JobType == EnumJobType.Recur)
        {
            if (string.IsNullOrEmpty(request.CronExpression))
            {
                result = Result<CreateJobResponse>.Fail(
                    "Cron Expression is required for Recur job type."
                );
                goto result;
            }
            if (string.IsNullOrEmpty(request.JobId))
            {
                result = Result<CreateJobResponse>.Fail("Job Id is required for Recur job type.");
                goto result;
            }
        }

        #endregion

        #endregion

        switch (request.JobType)
        {
            case EnumJobType.Delay:
                BackgroundJob.Schedule<IHttpClientService>(
                    x =>
                        x.SendAsync(
                            new JobSchedulerRequestDto
                            {
                                Uri = new Uri(request.Uri),
                                Endpoint = request.Endpoint,
                                JsonPayload = request.JsonPayload,
                            }
                        ),
                    request.DelayAt!.Value
                );
                break;
            case EnumJobType.Recur:
                RecurringJob.AddOrUpdate<IHttpClientService>(
                    request.JobId,
                    x =>
                        x.SendAsync(
                            new JobSchedulerRequestDto
                            {
                                Uri = new Uri(request.Uri),
                                Endpoint = request.Endpoint,
                                JsonPayload = request.JsonPayload,
                            }
                        ),
                    request.CronExpression,
                    TimeZoneInfo.Utc
                );
                break;
            case EnumJobType.FireAndForget:
                BackgroundJob.Enqueue<IHttpClientService>(x =>
                    x.SendAsync(
                        new JobSchedulerRequestDto
                        {
                            Uri = new Uri(request.Uri),
                            Endpoint = request.Endpoint,
                            JsonPayload = request.JsonPayload,
                        }
                    )
                );
                break;
            case EnumJobType.None:
            default:
                throw new ArgumentNullException(Result<object>.Fail("Invalid Job Type.").ToJson());
        }

        result = Result<CreateJobResponse>.Success();

    result:
        return result;
    }
}
