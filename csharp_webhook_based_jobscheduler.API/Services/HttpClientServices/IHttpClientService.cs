﻿namespace csharp_webhook_based_jobscheduler.API.Services.HttpClientServices;

public interface IHttpClientService
{
    Task SendAsync(JobSchedulerRequestDto schedulerRequestDto);
}
