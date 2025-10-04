using Microsoft.Extensions.DependencyInjection;
using TbSense.Backend.Services;
using TbSense.Backend.Abstraction.Interfaces;

namespace TbSense.Backend.Extensions;

public static class TbSenseBackendServicesExtension
{
    public static IServiceCollection AddTbSenseBackendServices(this IServiceCollection services)
    {
        services.AddScoped<IPlantationService, PlantationService>();
        services.AddScoped<IPlantationCoordinateService, PlantationCoordinateService>();
        services.AddScoped<ITreeService, TreeService>();
        services.AddScoped<ITreeMetricService, TreeMetricService>();
        services.AddScoped<IModelService, ModelService>();
        services.AddScoped<IPlantationYieldPredictionService, PlantationYieldPredictionService>();
        services.AddScoped<IPlantationHarvestService, PlantationHarvestService>();

        return services;
    }
}