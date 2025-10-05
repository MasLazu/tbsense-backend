using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IPlantationDashboardService
{
    Task<PlantationBasicSummaryResponse?> GetPlantationBasicSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationTreesSummaryResponse> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationHarvestSummaryResponse> GetPlantationHarvestCurrentMonthAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationHarvestSummaryResponse> GetPlantationHarvestCurrentYearAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationHarvestAllTimeResponse> GetPlantationHarvestAllTimeAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationRankingResponse?> GetPlantationRankingAsync(Guid plantationId, string period = "month", CancellationToken ct = default);
    Task<PlantationCurrentMetricsResponse?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationHealthStatusResponse> GetPlantationHealthStatusAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationEnvironmentalAveragesResponse> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days = 30, CancellationToken ct = default);
}
