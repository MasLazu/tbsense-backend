namespace TbSense.Backend.Interfaces;

/// <summary>
/// Repository interface for plantation-specific dashboard data
/// </summary>
public interface IPlantationDashboardRepository
{
    // Summary endpoints
    Task<(Guid id, string name, double landAreaHectares, DateTime plantedDate, int ageInDays, double ageInYears)?> GetPlantationBasicSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<(int totalTrees, double treesPerHectare, int activeTreesWithMetrics)> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<(double totalYieldKg, double yieldPerHectare, int harvestCount, double averagePerHarvest)> GetPlantationHarvestCurrentMonthAsync(Guid plantationId, CancellationToken ct = default);
    Task<(double totalYieldKg, double yieldPerHectare, int harvestCount)> GetPlantationHarvestCurrentYearAsync(Guid plantationId, CancellationToken ct = default);
    Task<(double totalYieldKg, double averageYieldPerHectare, int totalHarvests, DateTime? firstHarvestDate)> GetPlantationHarvestAllTimeAsync(Guid plantationId, CancellationToken ct = default);
    Task<(int rank, int totalPlantations, double percentile, string category)?> GetPlantationRankingAsync(Guid plantationId, DateTime startDate, DateTime endDate, CancellationToken ct = default);

    // Health & Metrics
    Task<(double temperature, double soilMoisture, double humidity, double lightIntensity, DateTime lastUpdated)?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default);
    Task<Dictionary<string, string>> GetPlantationHealthStatusAsync(Guid plantationId, CancellationToken ct = default);
    Task<Dictionary<string, (double avg, double min, double max)>> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days, CancellationToken ct = default);
}
