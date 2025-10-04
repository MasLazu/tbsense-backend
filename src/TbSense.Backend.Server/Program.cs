using TbSense.Backend.Server.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureTrainerService(builder.Configuration);
builder.Services.ConfigureStorageService(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
