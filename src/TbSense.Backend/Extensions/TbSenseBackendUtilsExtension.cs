using Microsoft.Extensions.DependencyInjection;
using TbSense.Backend.Utils;
using TbSense.Backend.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Utils;

namespace TbSense.Backend.Extensions;

public static class TbSenseBackendUtilsExtension
{
    public static IServiceCollection AddTbSenseBackendUtils(this IServiceCollection services)
    {
        RegisterPropertyMapsAndExpressionBuilders(services);

        return services;
    }

    private static void RegisterPropertyMapsAndExpressionBuilders(IServiceCollection services)
    {
        var entityPropertyMapPairs = new (Type entityType, Type propertyMapType)[]
        {
            (typeof(Plantation), typeof(PlantationEntityPropertyMap)),
            (typeof(PlantationCoordinate), typeof(PlantationCoordinateEntityPropertyMap)),
            (typeof(Tree), typeof(TreeEntityPropertyMap)),
            (typeof(TreeMetric), typeof(TreeMetricEntityPropertyMap)),
            (typeof(Model), typeof(ModelEntityPropertyMap)),
            (typeof(PlantationYieldPrediction), typeof(PlantationYieldPredictionEntityPropertyMap)),
            (typeof(PlantationHarvest), typeof(PlantationHarvestEntityPropertyMap))
        };

        foreach ((Type entityType, Type propertyMapType) in entityPropertyMapPairs)
        {
            Type propertyMapInterfaceType = typeof(IEntityPropertyMap<>).MakeGenericType(entityType);
            services.AddSingleton(propertyMapInterfaceType, propertyMapType);

            Type expressionBuilderType = typeof(ExpressionBuilder<>).MakeGenericType(entityType);
            services.AddScoped(expressionBuilderType);
        }
    }
}