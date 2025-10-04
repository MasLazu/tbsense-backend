using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using TbSense.Backend.Trainer.Abstraction.Interfaces;
using TbSense.Backend.Trainer.Abstraction.Models;

namespace TbSense.Backend.Trainer.Services;

public class TrainerService : ITrainerService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TrainerService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public TrainerService(
        HttpClient httpClient,
        ILogger<TrainerService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<TrainingResponse> TrainAsync(TrainingRequest request, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation(
                "Initiating training for model {ModelId} with {DataCount} training data rows",
                request.ModelId,
                request.TrainingData.Count);

            string jsonPayload = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/api/train", content, ct);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError(
                    "Training request failed with status {StatusCode}: {Error}",
                    response.StatusCode,
                    errorContent);

                return new TrainingResponse(
                    request.ModelId,
                    "Failed",
                    $"Training service returned error: {response.StatusCode}");
            }

            string responseContent = await response.Content.ReadAsStringAsync(ct);
            TrainingResponse? trainingResponse = JsonSerializer.Deserialize<TrainingResponse>(
                responseContent,
                _jsonOptions);

            if (trainingResponse == null)
            {
                _logger.LogError("Failed to deserialize training response");
                return new TrainingResponse(
                    request.ModelId,
                    "Failed",
                    "Failed to parse training service response");
            }

            _logger.LogInformation(
                "Training initiated successfully for model {ModelId}. Status: {Status}",
                request.ModelId,
                trainingResponse.Status);

            return trainingResponse;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while communicating with training service for model {ModelId}", request.ModelId);
            return new TrainingResponse(
                request.ModelId,
                "Failed",
                $"Connection error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Training request timed out for model {ModelId}", request.ModelId);
            return new TrainingResponse(
                request.ModelId,
                "Failed",
                "Request timed out");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while initiating training for model {ModelId}", request.ModelId);
            return new TrainingResponse(
                request.ModelId,
                "Failed",
                $"Unexpected error: {ex.Message}");
        }
    }

    public async Task<TrainingStatusResponse> GetTrainingStatusAsync(Guid modelId, CancellationToken ct = default)
    {
        try
        {
            _logger.LogInformation("Checking training status for model {ModelId}", modelId);

            HttpResponseMessage response = await _httpClient.GetAsync($"/api/train/status/{modelId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync(ct);
                _logger.LogError(
                    "Status check failed with status {StatusCode}: {Error}",
                    response.StatusCode,
                    errorContent);

                return new TrainingStatusResponse(
                    modelId,
                    "Unknown",
                    $"Failed to retrieve status: {response.StatusCode}",
                    null,
                    null,
                    null);
            }

            string responseContent = await response.Content.ReadAsStringAsync(ct);
            TrainingStatusResponse? statusResponse = JsonSerializer.Deserialize<TrainingStatusResponse>(
                responseContent,
                _jsonOptions);

            if (statusResponse == null)
            {
                _logger.LogError("Failed to deserialize status response for model {ModelId}", modelId);
                return new TrainingStatusResponse(
                    modelId,
                    "Unknown",
                    "Failed to parse status response",
                    null,
                    null,
                    null);
            }

            _logger.LogInformation(
                "Training status for model {ModelId}: {Status}",
                modelId,
                statusResponse.Status);

            return statusResponse;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error while checking status for model {ModelId}", modelId);
            return new TrainingStatusResponse(
                modelId,
                "Unknown",
                $"Connection error: {ex.Message}",
                null,
                null,
                null);
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogWarning(ex, "Status check timed out for model {ModelId}", modelId);
            return new TrainingStatusResponse(
                modelId,
                "Unknown",
                "Request timed out",
                null,
                null,
                null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while checking status for model {ModelId}", modelId);
            return new TrainingStatusResponse(
                modelId,
                "Unknown",
                $"Unexpected error: {ex.Message}",
                null,
                null,
                null);
        }
    }
}
