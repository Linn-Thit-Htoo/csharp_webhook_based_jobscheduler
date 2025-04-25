global using System.Text;
global using static System.Net.Mime.MediaTypeNames;

namespace csharp_webhook_based_jobscheduler.API.Services.HttpClientServices;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpClientService> _logger;

    public HttpClientService(
        IHttpClientFactory httpClientFactory,
        ILogger<HttpClientService> logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task SendAsync(JobSchedulerRequestDto schedulerRequestDto)
    {
        try
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new()
            {
                Content = new StringContent(
                    schedulerRequestDto.JsonPayload ?? string.Empty,
                    Encoding.UTF8,
                    Application.Json
                ),
                Method = HttpMethod.Post,
                RequestUri = new Uri(schedulerRequestDto.Uri, schedulerRequestDto.Endpoint),
                Headers =
                {
                    { "Accept", Application.Json },
                    { "Accept-Charset", "utf-8" },
                    { "User-Agent", "WebHookJobScheduler" },
                },
            };

            using HttpResponseMessage response = await client.SendAsync(
                httpRequestMessage,
                schedulerRequestDto.Cs
            );
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending HTTP request.");
            throw;
        }
    }
}
