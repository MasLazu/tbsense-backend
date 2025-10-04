using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IModelService : ICrudService<ModelDto, CreateModelRequest, UpdateModelRequest>
{
    /// <summary>
    /// Downloads a model file from storage
    /// </summary>
    /// <param name="modelId">The model ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Tuple containing the file stream and filename</returns>
    Task<(Stream stream, string fileName)> DownloadModelAsync(Guid modelId, CancellationToken ct = default);

    /// <summary>
    /// Initiates model training by preparing data and sending to ML service
    /// </summary>
    /// <param name="request">Training request with date range</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Model ID and status message</returns>
    Task<TrainModelResponse> TrainModelAsync(TrainModelRequest request, CancellationToken ct = default);

    /// <summary>
    /// Prepares training data by aggregating sensor metrics and harvest data
    /// </summary>
    /// <param name="startDate">Start date for training data</param>
    /// <param name="endDate">End date for training data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of processed data rows</returns>
    Task<List<ProcessedDataRow>> PrepareTrainingDataAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default);

    /// <summary>
    /// Completes model training by uploading model file and updating metrics
    /// </summary>
    /// <param name="modelId">The model ID</param>
    /// <param name="modelFile">The trained model file stream</param>
    /// <param name="fileName">The file name</param>
    /// <param name="accuracy">Model accuracy</param>
    /// <param name="mae">Mean Absolute Error</param>
    /// <param name="rmse">Root Mean Square Error</param>
    /// <param name="r2Score">R2 Score</param>
    /// <param name="ct">Cancellation token</param>
    Task CompleteTrainingAsync(
        Guid modelId,
        Stream modelFile,
        string fileName,
        double accuracy,
        double mae,
        double rmse,
        double r2Score,
        CancellationToken ct = default);
}
