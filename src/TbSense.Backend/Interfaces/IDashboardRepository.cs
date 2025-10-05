using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Interfaces;

/// <summary>
/// Repository interface for global dashboard data with database-side aggregation
/// </summary>
public interface IDashboardRepository
{
    // Summary endpoints
    Task<(int total, int active)> GetPlantationsSummaryAsync(CancellationToken ct = default);
    Task<(int total, double averagePerHectare)> GetTreesSummaryAsync(CancellationToken ct = default);
    Task<(double totalHectares, double utilized, double utilizationRate)> GetLandAreaSummaryAsync(CancellationToken ct = default);
    Task<(double totalYieldKg, double averageYieldPerHectare, int harvestCount)> GetHarvestCurrentMonthAsync(CancellationToken ct = default);
    Task<(double totalYieldKg, double averageYieldPerHectare, int harvestCount)> GetHarvestCurrentYearAsync(CancellationToken ct = default);

    // Environmental data
    Task<Dictionary<string, (double current, double[] optimal, string status)>> GetEnvironmentalAveragesAsync(CancellationToken ct = default);
    Task<(int plantationsInOptimalRange, int plantationsWithWarnings, int plantationsCritical, int total)> GetEnvironmentalStatusAsync(CancellationToken ct = default);
}
