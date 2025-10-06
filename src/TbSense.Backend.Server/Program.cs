using FastEndpoints;
using FastEndpoints.Swagger;
using MasLazu.AspNet.Framework.Application.Extensions;
using MasLazu.AspNet.Framework.EfCore.Extensions;
using MasLazu.AspNet.Framework.Endpoint.Extensions;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json;
using TbSense.Backend.EfCore.Extensions;
using TbSense.Backend.EfCore.Postgresql.Extensions;
using TbSense.Backend.Endpoints.Extensions;
using TbSense.Backend.Extensions;
using TbSense.Backend.Server.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure JSON options
builder.Services.Configure<JsonOptions>(o =>
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

// Add MasLazu.Framework
builder.Services.AddFrameworkApplication();
builder.Services.AddFrameworkEfCore();

// Add TbSense Backend services
builder.Services.AddTbSenseBackend(builder.Configuration);
builder.Services.AddTbSenseBackendEndpoints();

// Add EF Core with PostgreSQL
builder.Services.AddTbSenseBackendEfCore();
builder.Services.AddTbSenseBackendEfCorePostgresql(builder.Configuration);

// Add external services (Trainer, Storage)
builder.Services.ConfigureTrainerService(builder.Configuration);
builder.Services.ConfigureStorageService(builder.Configuration);

// Add FastEndpoints
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Add CORS
builder.Services.AddCors(options =>
{
    IConfigurationSection corsConfig = builder.Configuration.GetSection("Cors");
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(corsConfig.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>());
        policy.WithMethods(corsConfig.GetSection("AllowedMethods").Get<string[]>() ?? Array.Empty<string>());
        policy.WithHeaders(corsConfig.GetSection("AllowedHeaders").Get<string[]>() ?? Array.Empty<string>());
        if (corsConfig.GetValue<bool>("AllowCredentials"))
        {
            policy.AllowCredentials();
        }
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseCors();
app.UseFrameworkExceptionHandlerMiddleware();
app.UseFastEndpoints(c =>
{
    c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    c.Binding.UsePropertyNamingPolicy = true;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.Run();
