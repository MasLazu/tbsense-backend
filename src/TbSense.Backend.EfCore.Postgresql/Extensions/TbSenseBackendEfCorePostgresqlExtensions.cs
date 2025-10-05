using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data.Common;
using TbSense.Backend.EfCore.Data;
using MasLazu.AspNet.Framework.EfCore.Data;

namespace TbSense.Backend.EfCore.Postgresql.Extensions;

public static class TbSenseBackendEfCorePostgresqlExtensions
{
    public static IServiceCollection AddTbSenseBackendEfCorePostgresql(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not found in configuration");
        string readConnectionString = configuration.GetConnectionString("ReadConnection") ?? connectionString;

        services.AddScoped<DbConnection>(sp =>
        {
            var connection = new NpgsqlConnection(connectionString);
            return connection;
        });

        services.AddDbContext<TbSenseBackendDbContext>((sp, options) =>
        {
            DbConnection connection = sp.GetRequiredService<DbConnection>();
            options.UseNpgsql(connection).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<BaseDbContext>(sp => sp.GetRequiredService<TbSenseBackendDbContext>());

        services.AddDbContext<TbSenseBackendReadDbContext>(options =>
            options.UseNpgsql(readConnectionString)
                .UseSnakeCaseNamingConvention()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        return services;
    }
}
