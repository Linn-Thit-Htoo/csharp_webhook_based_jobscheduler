using csharp_webhook_based_jobscheduler.API.Configurations;
using csharp_webhook_based_jobscheduler.API.Services.HttpClientServices;
using FluentValidation;
using Hangfire;

namespace csharp_webhook_based_jobscheduler.API.Dependencies
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                opt.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHangfire(opt =>
            {
                opt.UseSqlServerStorage(builder.Configuration.GetConnectionString("DbConnection"))
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings();
            });

            builder.Services.AddHangfireServer();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly);
            });
            builder.Services.AddHealthChecks();
            builder.Services.Configure<AppSetting>(builder.Configuration);
            builder.Services.AddScoped<IHttpClientService, HttpClientService>();
            builder.Services.AddHttpClient();

            return services;
        }
    }
}
