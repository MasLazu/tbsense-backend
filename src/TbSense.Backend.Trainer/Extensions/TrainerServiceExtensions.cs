using Microsoft.Extensions.DependencyInjection;
using TbSense.Backend.Trainer.Abstraction.Interfaces;
using TbSense.Backend.Trainer.Services;

namespace TbSense.Backend.Trainer.Extensions;

public static class TrainerServiceExtensions
{
    public static IServiceCollection AddTrainerService(
        this IServiceCollection services,
        string baseAddress,
        TimeSpan? timeout = null)
    {
        services.AddHttpClient<ITrainerService, TrainerService>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
            client.Timeout = timeout ?? TimeSpan.FromMinutes(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }

    public static IServiceCollection AddTrainerService(
        this IServiceCollection services,
        Action<HttpClient> configureClient)
    {
        services.AddHttpClient<ITrainerService, TrainerService>(configureClient);

        return services;
    }
}
