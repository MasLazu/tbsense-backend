using TbSense.Backend.Trainer.Abstraction.Models;

namespace TbSense.Backend.Trainer.Abstraction.Interfaces;

/// <summary>
/// Service interface for ML model training operations
/// </summary>
public interface ITrainerService
{
    /// <summary>
    /// Sends training data to the ML training service and initiates training
    /// </summary>
    /// <param name="request">Training request containing model ID and training data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Training response with job status</returns>
    Task<TrainingResponse> TrainAsync(TrainingRequest request, CancellationToken ct = default);

    /// <summary>
    /// Checks the status of a training job
    /// </summary>
    /// <param name="modelId">The model ID to check status for</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Training status information</returns>
    Task<TrainingStatusResponse> GetTrainingStatusAsync(Guid modelId, CancellationToken ct = default);
}
