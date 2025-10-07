using TbSense.Backend.Domain.Entities;
using TbSense.Backend.EfCore.Data;
using TbSense.Backend.EfCore.Repositories;
using TbSense.Backend.Interfaces;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.EfCore.Data;
using MasLazu.AspNet.Framework.EfCore.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace TbSense.Backend.EfCore.Extensions;

public static class TbSenseBackendEfCoreExtensions
{
    public static IServiceCollection AddTbSenseBackendEfCore(this IServiceCollection services)
    {
        services.AddScoped<IRepository<Plantation>, Repository<Plantation, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<PlantationCoordinate>, Repository<PlantationCoordinate, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<Tree>, Repository<Tree, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<TreeMetric>, Repository<TreeMetric, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<Model>, Repository<Model, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<PlantationYieldPrediction>, Repository<PlantationYieldPrediction, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<PlantationHarvest>, Repository<PlantationHarvest, TbSenseBackendDbContext>>();

        // AI entities
        services.AddScoped<IRepository<AiSession>, Repository<AiSession, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<AiSessionChat>, Repository<AiSessionChat, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<SystemPrompt>, Repository<SystemPrompt, TbSenseBackendDbContext>>();
        services.AddScoped<IRepository<KnowledgeBase>, Repository<KnowledgeBase, TbSenseBackendDbContext>>();

        services.AddScoped<IReadRepository<Plantation>, ReadRepository<Plantation, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<PlantationCoordinate>, ReadRepository<PlantationCoordinate, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<Tree>, ReadRepository<Tree, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<TreeMetric>, ReadRepository<TreeMetric, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<Model>, ReadRepository<Model, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<PlantationYieldPrediction>, ReadRepository<PlantationYieldPrediction, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<PlantationHarvest>, ReadRepository<PlantationHarvest, TbSenseBackendReadDbContext>>();

        // AI entities read repositories
        services.AddScoped<IReadRepository<AiSession>, ReadRepository<AiSession, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<AiSessionChat>, ReadRepository<AiSessionChat, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<SystemPrompt>, ReadRepository<SystemPrompt, TbSenseBackendReadDbContext>>();
        services.AddScoped<IReadRepository<KnowledgeBase>, ReadRepository<KnowledgeBase, TbSenseBackendReadDbContext>>();

        // Custom repositories
        services.AddScoped<ITrainingDataRepository, TrainingDataRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<IPlantationDashboardRepository, PlantationDashboardRepository>();
        services.AddScoped<ITreeDashboardRepository, TreeDashboardRepository>();

        services.AddScoped<BaseDbContext>(sp => sp.GetRequiredService<TbSenseBackendDbContext>());

        return services;
    }
}