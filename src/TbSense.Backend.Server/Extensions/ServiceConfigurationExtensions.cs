using Microsoft.Extensions.Configuration;
using TbSense.Backend.Trainer.Extensions;
using TbSense.Backend.Storage.Extensions;

namespace TbSense.Backend.Server.Extensions;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection ConfigureTrainerService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string baseUrl = configuration["TrainerService:BaseUrl"]
            ?? throw new InvalidOperationException("TrainerService:BaseUrl configuration is missing");

        int timeoutMinutes = configuration.GetValue<int>("TrainerService:TimeoutMinutes", 30);

        services.AddTrainerService(
            baseUrl,
            TimeSpan.FromMinutes(timeoutMinutes));

        return services;
    }

    public static IServiceCollection ConfigureStorageService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStorageService(configuration);

        return services;
    }
}
