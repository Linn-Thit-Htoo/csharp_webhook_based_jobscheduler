using csharp_webhook_based_jobscheduler.API.Enums;
using FluentValidation;

namespace csharp_webhook_based_jobscheduler.API.Features.JobScheduler.CreateJob
{
    public class CreateJobValidator : AbstractValidator<CreateJobCommand>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.JobType)
                .NotNull().NotEmpty()
                .IsInEnum()
                .WithMessage("Job Type is invalid.");

            RuleFor(x => x.Uri)
                .NotNull().NotEmpty()
                .WithMessage("Invalid Uri.");

            RuleFor(x => x.Endpoint)
                .NotNull().NotEmpty()
                .WithMessage("Invalid endpoint.");
        }
    }
}
