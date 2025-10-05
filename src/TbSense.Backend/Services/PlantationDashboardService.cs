using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Interfaces;

namespace TbSense.Backend.Services;

public class PlantationDashboardService : IPlantationDashboardService
{
    private readonly IPlantationDashboardRepository _repository;

    public PlantationDashboardService(IPlantationDashboardRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PlantationBasicSummaryResponse?> GetPlantationBasicSummaryAsync(Guid plantationId, CancellationToken ct = default)
    {
        (Guid id, string name, double landAreaHectares, DateTime plantedDate, int ageInDays, double ageInYears)? result = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);

        if (result == null)
        {
            return null;
        }

        return new PlantationBasicSummaryResponse(
            result.Value.id,
            result.Value.name,
            result.Value.landAreaHectares,
            result.Value.plantedDate,
            result.Value.ageInDays,
            result.Value.ageInYears
        );
    }

    public async Task<PlantationTreesSummaryResponse> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default)
    {
        (int totalTrees, double treesPerHectare, int activeTreesWithMetrics) = await _repository.GetPlantationTreesSummaryAsync(plantationId, ct);
        return new PlantationTreesSummaryResponse(totalTrees, treesPerHectare, activeTreesWithMetrics);
    }

    public async Task<PlantationHarvestSummaryResponse> GetPlantationHarvestCurrentMonthAsync(Guid plantationId, CancellationToken ct = default)
    {
        (double totalYieldKg, double yieldPerHectare, int harvestCount, double averagePerHarvest) = await _repository.GetPlantationHarvestCurrentMonthAsync(plantationId, ct);
        return new PlantationHarvestSummaryResponse(totalYieldKg, yieldPerHectare, harvestCount, averagePerHarvest);
    }

    public async Task<PlantationHarvestSummaryResponse> GetPlantationHarvestCurrentYearAsync(Guid plantationId, CancellationToken ct = default)
    {
        (double totalYieldKg, double yieldPerHectare, int harvestCount) = await _repository.GetPlantationHarvestCurrentYearAsync(plantationId, ct);
        return new PlantationHarvestSummaryResponse(totalYieldKg, yieldPerHectare, harvestCount);
    }

    public async Task<PlantationHarvestAllTimeResponse> GetPlantationHarvestAllTimeAsync(Guid plantationId, CancellationToken ct = default)
    {
        (double totalYieldKg, double averageYieldPerHectare, int totalHarvests, DateTime? firstHarvestDate) = await _repository.GetPlantationHarvestAllTimeAsync(plantationId, ct);
        return new PlantationHarvestAllTimeResponse(totalYieldKg, averageYieldPerHectare, totalHarvests, firstHarvestDate);
    }

    public async Task<PlantationRankingResponse?> GetPlantationRankingAsync(Guid plantationId, string period = "month", CancellationToken ct = default)
    {
        (DateTime startDate, DateTime endDate) = GetDateRangeForPeriod(period);
        (int rank, int totalPlantations, double percentile, string category)? result = await _repository.GetPlantationRankingAsync(plantationId, startDate, endDate, ct);

        if (result == null)
        {
            return null;
        }

        return new PlantationRankingResponse(
            result.Value.rank,
            result.Value.totalPlantations,
            result.Value.percentile,
            result.Value.category
        );
    }

    public async Task<PlantationCurrentMetricsResponse?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default)
    {
        (double temperature, double soilMoisture, double humidity, double lightIntensity, DateTime lastUpdated)? result = await _repository.GetPlantationCurrentMetricsAsync(plantationId, ct);

        if (result == null)
        {
            return null;
        }

        return new PlantationCurrentMetricsResponse(
            result.Value.temperature,
            result.Value.soilMoisture,
            result.Value.humidity,
            result.Value.lightIntensity,
            result.Value.lastUpdated
        );
    }

    public async Task<PlantationHealthStatusResponse> GetPlantationHealthStatusAsync(Guid plantationId, CancellationToken ct = default)
    {
        Dictionary<string, string> status = await _repository.GetPlantationHealthStatusAsync(plantationId, ct);
        return new PlantationHealthStatusResponse(status);
    }

    public async Task<PlantationEnvironmentalAveragesResponse> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days = 30, CancellationToken ct = default)
    {
        Dictionary<string, (double avg, double min, double max)> metrics = await _repository.GetPlantationEnvironmentalAveragesAsync(plantationId, days, ct);

        var response = metrics.ToDictionary(
            kvp => kvp.Key,
            kvp => new EnvironmentalAverageResponse(kvp.Value.avg, kvp.Value.min, kvp.Value.max)
        );

        return new PlantationEnvironmentalAveragesResponse(response);
    }

    private static (DateTime startDate, DateTime endDate) GetDateRangeForPeriod(string period)
    {
        DateTime now = DateTime.UtcNow;

        return period.ToLower() switch
        {
            "week" => (now.AddDays(-7), now),
            "month" => (new DateTime(now.Year, now.Month, 1), now),
            "quarter" => (now.AddMonths(-3), now),
            "year" => (new DateTime(now.Year, 1, 1), now),
            _ => (new DateTime(now.Year, now.Month, 1), now) // default to month
        };
    }
}
