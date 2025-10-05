using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IDashboardService
{
    Task<PlantationsSummaryResponse> GetPlantationsSummaryAsync(CancellationToken ct = default);
    Task<TreesSummaryResponse> GetTreesSummaryAsync(CancellationToken ct = default);
    Task<LandAreaSummaryResponse> GetLandAreaSummaryAsync(CancellationToken ct = default);
    Task<HarvestSummaryResponse> GetHarvestCurrentMonthAsync(CancellationToken ct = default);
    Task<HarvestSummaryResponse> GetHarvestCurrentYearAsync(CancellationToken ct = default);
    Task<EnvironmentalAveragesResponse> GetEnvironmentalAveragesAsync(CancellationToken ct = default);
    Task<EnvironmentalStatusResponse> GetEnvironmentalStatusAsync(CancellationToken ct = default);
}
