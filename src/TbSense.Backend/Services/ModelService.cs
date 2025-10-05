using FluentValidation;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Domain.Entities;
using TbSense.Backend.Storage.Abstraction.Interfaces;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Services;
using MasLazu.AspNet.Framework.Application.Exceptions;
using Microsoft.Extensions.Logging;
using TbSense.Backend.Trainer.Abstraction.Interfaces;
using TbSense.Backend.Trainer.Abstraction.Models;

namespace TbSense.Backend.Services;

public class ModelService : CrudService<Model, ModelDto, CreateModelRequest, UpdateModelRequest>, IModelService
{
    private readonly IStorageService _storageService;
    private readonly IReadRepository<Plantation> _plantationRepository;
    private readonly IReadRepository<Tree> _treeRepository;
    private readonly IReadRepository<TreeMetric> _treeMetricRepository;
    private readonly IReadRepository<PlantationHarvest> _harvestRepository;
    private readonly ITrainerService _trainerService;
    private readonly ILogger<ModelService> _logger;

    public ModelService(
        IRepository<Model> repository,
        IReadRepository<Model> readRepository,
        IUnitOfWork unitOfWork,
        IEntityPropertyMap<Model> propertyMap,
        IPaginationValidator<Model> paginationValidator,
        ICursorPaginationValidator<Model> cursorPaginationValidator,
        IStorageService storageService,
        IReadRepository<Plantation> plantationRepository,
        IReadRepository<Tree> treeRepository,
        IReadRepository<TreeMetric> treeMetricRepository,
        IReadRepository<PlantationHarvest> harvestRepository,
        ITrainerService trainerService,
        ILogger<ModelService> logger,
        IValidator<CreateModelRequest>? createValidator = null,
        IValidator<UpdateModelRequest>? updateValidator = null)
        : base(repository, readRepository, unitOfWork, propertyMap, paginationValidator, cursorPaginationValidator, createValidator, updateValidator)
    {
        _storageService = storageService;
        _plantationRepository = plantationRepository;
        _treeRepository = treeRepository;
        _treeMetricRepository = treeMetricRepository;
        _harvestRepository = harvestRepository;
        _trainerService = trainerService;
        _logger = logger;
    }

    public async Task<(Stream stream, string fileName)> DownloadModelAsync(Guid modelId, CancellationToken ct = default)
    {
        ModelDto? model = await GetByIdAsync(modelId, ct) ??
            throw new NotFoundException($"Model with ID {modelId} not found");

        bool exists = await _storageService.ExistsAsync(model.FilePath, ct);
        if (!exists)
        {
            throw new NotFoundException($"Model file for ID {modelId} not found in storage");
        }

        Stream stream = await _storageService.DownloadAsync(model.FilePath, ct);

        string fileName = Path.GetFileName(model.FilePath) ?? $"model_{modelId}.onnx";

        return (stream, fileName);
    }

    public async Task<TrainModelResponse> TrainModelAsync(TrainModelRequest request, CancellationToken ct = default)
    {
        _logger.LogInformation("Starting model training for date range {Start} to {End}",
            request.TrainingDataStart, request.TrainingDataEnd);

        // Validate date range
        if (request.TrainingDataStart >= request.TrainingDataEnd)
        {
            throw new BadRequestException("Training data start date must be before end date");
        }

        // Create a new model record with "Training" status
        var createModelRequest = new CreateModelRequest(
            TrainingStatus: "Training",
            FilePath: string.Empty, // Will be updated when training completes
            IsUsed: false,
            TrainingDataStart: request.TrainingDataStart,
            TrainingDataEnd: request.TrainingDataEnd,
            Accuracy: null,
            MAE: null,
            RMSE: null,
            R2Score: null
        );

        ModelDto model = await CreateAsync(createModelRequest, true, ct);
        _logger.LogInformation("Created model record with ID {ModelId}", model.Id);

        // Prepare training data
        List<ProcessedDataRow> trainingData = await PrepareTrainingDataAsync(
            request.TrainingDataStart,
            request.TrainingDataEnd,
            ct);

        _logger.LogInformation("Prepared {Count} training data rows", trainingData.Count);

        if (trainingData.Count == 0)
        {
            throw new BadRequestException("No training data available for the specified date range");
        }

        // Convert ProcessedDataRow to TrainingDataRow
        var trainerData = trainingData.Select(row => new TrainingDataRow(
            PlantationId: row.PlantationId,
            PlantationAgeDays: row.PlantationAgeDays,
            AvgTemperature: row.AvgTemperature,
            AvgHumidity: row.AvgHumidity,
            AvgSoilMoisture: row.AvgSoilMoisture,
            AvgLightIntensity: row.AvgLightIntensity,
            TreesPerHectare: row.TreesPerHectare,
            YieldPerHectarePerMonth: row.YieldPerHectarePerMonth,
            PeriodStart: row.PeriodStart,
            PeriodEnd: row.PeriodEnd
        )).ToList();

        var trainingRequest = new TrainingRequest(model.Id, trainerData);
        TrainingResponse trainerResponse = await _trainerService.TrainAsync(trainingRequest, ct);

        _logger.LogInformation(
            "Training service response for model {ModelId}: Status={Status}, Message={Message}",
            model.Id,
            trainerResponse.Status,
            trainerResponse.Message);

        if (trainerResponse.Status == "Failed")
        {
            var failedUpdateRequest = new UpdateModelRequest(
                Id: model.Id,
                TrainingStatus: "Failed",
                FilePath: null,
                IsUsed: null,
                TrainingDataStart: null,
                TrainingDataEnd: null,
                Accuracy: null,
                MAE: null,
                RMSE: null,
                R2Score: null
            );
            await UpdateAsync(failedUpdateRequest, true, ct);

            throw new BadRequestException($"Training initiation failed: {trainerResponse.Message}");
        }

        return new TrainModelResponse(model.Id, trainerResponse.Message);
    }

    public async Task<List<ProcessedDataRow>> PrepareTrainingDataAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Preparing training data from {Start} to {End}", startDate, endDate);

        IEnumerable<Plantation> plantations = await _plantationRepository.FindAsync(
            p => true,
            p => p.Trees!,
            p => p.Harvests!
        );

        var processedData = new List<ProcessedDataRow>();

        foreach (Plantation plantation in plantations)
        {
            int plantationAgeDays = (startDate - plantation.PlantedDate).Days;

            if (plantationAgeDays < 0)
            {
                continue;
            }

            List<Guid> treeIds = plantation.Trees?.Select(t => t.Id).ToList() ?? new List<Guid>();

            if (treeIds.Count == 0)
            {
                continue;
            }

            IEnumerable<TreeMetric> metrics = await _treeMetricRepository.FindAsync(
                m => treeIds.Contains(m.TreeId) &&
                     m.CreatedAt >= startDate &&
                     m.CreatedAt <= endDate,
                ct);

            var metricsList = metrics.ToList();

            if (metricsList.Count == 0)
            {
                continue;
            }

            double avgAirTemperature = metricsList.Average(m => m.AirTemperature);
            double avgSoilTemperature = metricsList.Average(m => m.SoilTemperature);
            double avgSoilMoisture = metricsList.Average(m => m.SoilMoisture);
            double avgTemperature = avgAirTemperature;
            double avgHumidity = 0;
            double avgLightIntensity = 0;

            IEnumerable<PlantationHarvest> harvests = await _harvestRepository.FindAsync(
                h => h.PlantationId == plantation.Id &&
                     h.HarvestDate >= startDate &&
                     h.HarvestDate <= endDate,
                ct);

            double totalYield = harvests.Sum(h => h.YieldKg);

            if (totalYield > 0)
            {
                // Calculate trees per hectare
                int treeCount = plantation.Trees?.Count ?? 0;
                double treesPerHectare = plantation.LandAreaHectares > 0
                    ? treeCount / (double)plantation.LandAreaHectares
                    : 0;

                // Calculate the number of months in the period
                double periodMonths = (endDate - startDate).TotalDays / 30.0; // Approximate month length
                if (periodMonths == 0)
                {
                    periodMonths = 1; // Avoid division by zero
                }

                // Calculate yield per hectare per month
                double yieldPerHectarePerMonth = plantation.LandAreaHectares > 0
                    ? totalYield / plantation.LandAreaHectares / periodMonths
                    : 0;

                processedData.Add(new ProcessedDataRow(
                    PlantationId: plantation.Id,
                    PlantationAgeDays: plantationAgeDays,
                    AvgTemperature: avgTemperature,
                    AvgHumidity: avgHumidity,
                    AvgSoilMoisture: avgSoilMoisture,
                    AvgLightIntensity: avgLightIntensity,
                    TreesPerHectare: treesPerHectare,
                    YieldPerHectarePerMonth: yieldPerHectarePerMonth,
                    PeriodStart: startDate,
                    PeriodEnd: endDate
                ));
            }
        }

        _logger.LogInformation("Prepared {Count} processed data rows", processedData.Count);
        return processedData;
    }

    public async Task CompleteTrainingAsync(
        Guid modelId,
        Stream modelFile,
        string fileName,
        double accuracy,
        double mae,
        double rmse,
        double r2Score,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Completing training for model {ModelId}", modelId);

        ModelDto? model = await GetByIdAsync(modelId, ct) ??
            throw new NotFoundException($"Model with ID {modelId} not found");

        if (model.TrainingStatus != "Training")
        {
            throw new BadRequestException($"Model {modelId} is not in training status");
        }

        string fileExtension = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(fileExtension))
        {
            fileExtension = ".onnx";
        }

        string filePath = $"models/model_{modelId}{fileExtension}";

        try
        {
            _logger.LogInformation("Uploading model file to storage: {FilePath}", filePath);
            await _storageService.UploadAsync(filePath, modelFile, ct);
            _logger.LogInformation("Model file uploaded successfully");

            var updateRequest = new UpdateModelRequest(
                Id: modelId,
                TrainingStatus: "Completed",
                FilePath: filePath,
                IsUsed: null,
                TrainingDataStart: null,
                TrainingDataEnd: null,
                Accuracy: accuracy,
                MAE: mae,
                RMSE: rmse,
                R2Score: r2Score
            );

            await UpdateAsync(updateRequest, true, ct);
            _logger.LogInformation("Model {ModelId} training completed successfully", modelId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing training for model {ModelId}", modelId);

            var failedUpdateRequest = new UpdateModelRequest(
                Id: modelId,
                TrainingStatus: "Failed",
                FilePath: null,
                IsUsed: null,
                TrainingDataStart: null,
                TrainingDataEnd: null,
                Accuracy: null,
                MAE: null,
                RMSE: null,
                R2Score: null
            );

            await UpdateAsync(failedUpdateRequest, true, ct);
            throw;
        }
    }
}
