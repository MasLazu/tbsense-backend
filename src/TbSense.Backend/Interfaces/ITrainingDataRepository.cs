using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Interfaces;

/// <summary>
/// Repository interface for efficiently retrieving training data with database-side aggregation
/// </summary>
public interface ITrainingDataRepository
{
    /// <summary>
    /// Retrieves processed training data with all calculations performed at the database level
    /// to avoid loading large datasets into memory
    /// </summary>
    /// <param name="startDate">Start date of the training period</param>
    /// <param name="endDate">End date of the training period</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of processed training data rows</returns>
    Task<List<ProcessedDataRow>> GetTrainingDataAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default);
}
