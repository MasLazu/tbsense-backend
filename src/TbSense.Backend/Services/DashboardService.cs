using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Interfaces;

namespace TbSense.Backend.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PlantationsSummaryResponse> GetPlantationsSummaryAsync(CancellationToken ct = default)
    {
        (int total, int active) = await _repository.GetPlantationsSummaryAsync(ct);
        return new PlantationsSummaryResponse(total, active);
    }

    public async Task<TreesSummaryResponse> GetTreesSummaryAsync(CancellationToken ct = default)
    {
        (int total, double averagePerHectare) = await _repository.GetTreesSummaryAsync(ct);
        return new TreesSummaryResponse(total, averagePerHectare);
    }

    public async Task<LandAreaSummaryResponse> GetLandAreaSummaryAsync(CancellationToken ct = default)
    {
        (double totalHectares, double utilized, double utilizationRate) = await _repository.GetLandAreaSummaryAsync(ct);
        return new LandAreaSummaryResponse(totalHectares, utilized, utilizationRate);
    }

    public async Task<HarvestSummaryResponse> GetHarvestCurrentMonthAsync(CancellationToken ct = default)
    {
        (double totalYieldKg, double averageYieldPerHectare, int harvestCount) = await _repository.GetHarvestCurrentMonthAsync(ct);
        return new HarvestSummaryResponse(totalYieldKg, averageYieldPerHectare, harvestCount);
    }

    public async Task<HarvestSummaryResponse> GetHarvestCurrentYearAsync(CancellationToken ct = default)
    {
        (double totalYieldKg, double averageYieldPerHectare, int harvestCount) = await _repository.GetHarvestCurrentYearAsync(ct);
        return new HarvestSummaryResponse(totalYieldKg, averageYieldPerHectare, harvestCount);
    }

    public async Task<EnvironmentalAveragesResponse> GetEnvironmentalAveragesAsync(CancellationToken ct = default)
    {
        Dictionary<string, (double current, double[] optimal, string status)> metrics = await _repository.GetEnvironmentalAveragesAsync(ct);

        var response = metrics.ToDictionary(
            kvp => kvp.Key,
            kvp => new EnvironmentalMetricResponse(kvp.Value.current, kvp.Value.optimal, kvp.Value.status)
        );

        return new EnvironmentalAveragesResponse(response);
    }

    public async Task<EnvironmentalStatusResponse> GetEnvironmentalStatusAsync(CancellationToken ct = default)
    {
        (int optimal, int warnings, int critical, int total) = await _repository.GetEnvironmentalStatusAsync(ct);
        return new EnvironmentalStatusResponse(optimal, warnings, critical, total);
    }
}
