using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Interfaces;

/// <summary>
/// Repository interface for plantation-specific dashboard data
/// </summary>
public interface IPlantationDashboardRepository
{
    // Summary endpoints
    Task<PlantationBasicSummaryResponse?> GetPlantationBasicSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationTreesSummaryResponse> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationHarvestSummaryResponse> GetPlantationHarvestSummaryAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<PlantationRankingResponse?> GetPlantationRankingAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Health & Metrics
    Task<PlantationCurrentMetricsResponse?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationEnvironmentalMetrics> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days, CancellationToken ct = default);

    // Timeseries data
    Task<List<PlantationEnvironmentalTimeseriesDataPoint>> GetPlantationEnvironmentalTimeseriesAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<List<PlantationHarvestTimeseriesDataPoint>> GetPlantationHarvestTimeseriesAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);
    Task<List<PlantationTreeGrowthDataPoint>> GetPlantationTreeGrowthTimeseriesAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);

    // Chart data
    Task<List<PlantationTreeActivityItem>> GetPlantationTreeActivityChartAsync(Guid plantationId, CancellationToken ct = default);
    Task<List<MonthlyHarvestDistributionItem>> GetPlantationHarvestByMonthChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<EnvironmentalZoneItem>> GetPlantationEnvironmentalZonesChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Bar chart data
    Task<List<TopTreeByMetricItem>> GetTopTreesByMetricChartAsync(Guid plantationId, string metricType, DateTime? startDate = null, DateTime? endDate = null, int limit = 10, CancellationToken ct = default);
    Task<List<MonthlyHarvestComparisonItem>> GetMonthlyHarvestComparisonChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<MonthlyTreeActivityItem>> GetTreeActivityTrendChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<TreesByZoneComparisonItem>> GetTreesByZoneChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<WeeklyHarvestPerformanceItem>> GetWeeklyHarvestPerformanceChartAsync(Guid plantationId, int weeks = 12, CancellationToken ct = default);

    // Histogram data
    Task<List<double>> GetTreeYieldDistributionDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetAirTemperatureDistributionDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetSoilTemperatureDistributionDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetSoilMoistureDistributionDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetTreeAgeDistributionDataAsync(Guid plantationId, CancellationToken ct = default);

    // Area chart data
    Task<List<(DateTime Date, double Yield)>> GetCumulativeHarvestYieldDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, int Count)>> GetCumulativeHarvestCountDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, int Count)>> GetTreeMonitoringAdoptionDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, int Count)>> GetCumulativeMetricReadingsDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, string MetricType, double Value)>> GetStackedMetricsByTypeDataAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
}


