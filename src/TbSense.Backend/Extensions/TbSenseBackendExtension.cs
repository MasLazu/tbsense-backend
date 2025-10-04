using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Services;

namespace TbSense.Backend.Extensions;

public static class TbSenseBackendExtension
{
    public static IServiceCollection AddTbSenseBackend(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTbSenseBackendServices();
        services.AddTbSenseBackendUtils();
        services.AddTbSenseBackendValidators();

        return services;
    }
}