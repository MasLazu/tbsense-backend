using Microsoft.Extensions.DependencyInjection;
using TbSense.Backend.Validators;
using FluentValidation;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Extensions;

public static class TbSenseBackendValidatorsExtension
{
    public static IServiceCollection AddTbSenseBackendValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreatePlantationRequest>, CreatePlantationRequestValidator>();
        services.AddScoped<IValidator<UpdatePlantationRequest>, UpdatePlantationRequestValidator>();
        services.AddScoped<IValidator<CreatePlantationCoordinateRequest>, CreatePlantationCoordinateRequestValidator>();
        services.AddScoped<IValidator<UpdatePlantationCoordinateRequest>, UpdatePlantationCoordinateRequestValidator>();
        services.AddScoped<IValidator<CreateTreeRequest>, CreateTreeRequestValidator>();
        services.AddScoped<IValidator<UpdateTreeRequest>, UpdateTreeRequestValidator>();
        services.AddScoped<IValidator<CreateTreeMetricRequest>, CreateTreeMetricRequestValidator>();
        services.AddScoped<IValidator<UpdateTreeMetricRequest>, UpdateTreeMetricRequestValidator>();
        services.AddScoped<IValidator<CreateModelRequest>, CreateModelRequestValidator>();
        services.AddScoped<IValidator<UpdateModelRequest>, UpdateModelRequestValidator>();
        services.AddScoped<IValidator<CreatePlantationYieldPredictionRequest>, CreatePlantationYieldPredictionRequestValidator>();
        services.AddScoped<IValidator<UpdatePlantationYieldPredictionRequest>, UpdatePlantationYieldPredictionRequestValidator>();
        services.AddScoped<IValidator<CreatePlantationHarvestRequest>, CreatePlantationHarvestRequestValidator>();
        services.AddScoped<IValidator<UpdatePlantationHarvestRequest>, UpdatePlantationHarvestRequestValidator>();

        // AI validators
        services.AddScoped<IValidator<CreateAiSessionRequest>, CreateAiSessionRequestValidator>();
        services.AddScoped<IValidator<UpdateAiSessionRequest>, UpdateAiSessionRequestValidator>();
        services.AddScoped<IValidator<CreateAiSessionChatRequest>, CreateAiSessionChatRequestValidator>();
        services.AddScoped<IValidator<UpdateAiSessionChatRequest>, UpdateAiSessionChatRequestValidator>();
        services.AddScoped<IValidator<CreateSystemPromptRequest>, CreateSystemPromptRequestValidator>();
        services.AddScoped<IValidator<UpdateSystemPromptRequest>, UpdateSystemPromptRequestValidator>();
        services.AddScoped<IValidator<CreateKnowledgeBaseRequest>, CreateKnowledgeBaseRequestValidator>();
        services.AddScoped<IValidator<UpdateKnowledgeBaseRequest>, UpdateKnowledgeBaseRequestValidator>();

        return services;
    }
}