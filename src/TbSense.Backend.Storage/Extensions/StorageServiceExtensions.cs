using FluentStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TbSense.Backend.Storage.Abstraction.Interfaces;
using TbSense.Backend.Storage.Services;

namespace TbSense.Backend.Storage.Extensions;

public static class StorageServiceExtensions
{
    public static IServiceCollection AddStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IStorageService, StorageService>();

        services.AddSingleton(sp =>
        {
            IConfigurationSection minioConfig = configuration.GetSection("MinIO");
            string endpoint = minioConfig["Endpoint"] ?? "localhost:9000";
            string accessKey = minioConfig["AccessKey"] ?? "minioadmin";
            string secretKey = minioConfig["SecretKey"] ?? "minioadmin";
            string bucketName = minioConfig["BucketName"] ?? "models";
            bool useSSL = minioConfig.GetValue("UseSSL", false);

            string connectionString = $"s3://{accessKey}:{secretKey}@{endpoint}/{bucketName}?ssl={useSSL}";

            return StorageFactory.Blobs.FromConnectionString(connectionString);
        });

        return services;
    }
}