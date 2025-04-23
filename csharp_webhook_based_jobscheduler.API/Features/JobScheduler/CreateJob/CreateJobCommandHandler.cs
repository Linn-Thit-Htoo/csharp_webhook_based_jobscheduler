using csharp_webhook_based_jobscheduler.API.Constants;
using csharp_webhook_based_jobscheduler.API.Models;
using csharp_webhook_based_jobscheduler.API.Services.HttpClientServices;
using csharp_webhook_based_jobscheduler.API.Utils;
using FluentValidation;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace csharp_webhook_based_jobscheduler.API.Features.JobScheduler.CreateJob
{
    public class CreateJobCommandHandler : IRequestHandler<CreateJobCommand, Result<CreateJobResponse>>
    {
        private readonly IValidator<CreateJobCommand> _createJobValidator;

        public CreateJobCommandHandler(IValidator<CreateJobCommand> createJobValidator)
        {
            _createJobValidator = createJobValidator;
        }

        public async Task<Result<CreateJobResponse>> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            Result<CreateJobResponse> result;

            var validationResult = await _createJobValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                result = Result<CreateJobResponse>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToString()!);
                goto result;
            }

            switch (request.JobType)
            {
                case Enums.EnumJobType.Delay:
                    BackgroundJob.Schedule<IHttpClientService>(x =>
                        x.SendAsync(new JobSchedulerRequestDto
                        {
                            Uri = new Uri(request.Uri),
                            Endpoint = request.Endpoint,
                            JsonPayload = request.JsonPayload,
                        }), request.DelayAt!.Value);
                    break;
                case Enums.EnumJobType.Recur:
                    RecurringJob.AddOrUpdate<IHttpClientService>(request.JobId, x =>
                        x.SendAsync(new JobSchedulerRequestDto
                        {
                            Uri = new Uri(request.Uri),
                            Endpoint = request.Endpoint,
                            JsonPayload = request.JsonPayload,
                        }), request.CronExpression!, TimeZoneInfo.Utc);
                    break;
                case Enums.EnumJobType.FireAndForget:
                    BackgroundJob.Enqueue<IHttpClientService>(x =>
                        x.SendAsync(new JobSchedulerRequestDto
                        {
                            Uri = new Uri(request.Uri),
                            Endpoint = request.Endpoint,
                            JsonPayload = request.JsonPayload,
                        }));
                    break;
                case Enums.EnumJobType.None:
                default:
                    break;
            }

            result = Result<CreateJobResponse>.Success();

        result:
            return result;
        }
    }
}
