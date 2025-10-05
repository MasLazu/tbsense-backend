using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IPlantationDashboardService
{
    Task<PlantationBasicSummaryResponse?> GetPlantationBasicSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationTreesSummaryResponse> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationHarvestSummaryResponse> GetPlantationHarvestSummaryAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<PlantationRankingResponse?> GetPlantationRankingAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<PlantationCurrentMetricsResponse?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default);
    Task<PlantationEnvironmentalAveragesResponse> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days = 30, CancellationToken ct = default);

    // Timeseries endpoints
    Task<PlantationEnvironmentalTimeseriesResponse> GetPlantationEnvironmentalTimeseriesAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<PlantationHarvestTimeseriesResponse> GetPlantationHarvestTimeseriesAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);
    Task<PlantationTreeGrowthTimeseriesResponse> GetPlantationTreeGrowthTimeseriesAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);

    // Chart endpoints
    Task<PlantationTreeActivityResponse> GetPlantationTreeActivityChartAsync(Guid plantationId, CancellationToken ct = default);
    Task<MonthlyHarvestDistributionResponse> GetPlantationHarvestByMonthChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<EnvironmentalZonesResponse> GetPlantationEnvironmentalZonesChartAsync(Guid plantationId, int days = 30, CancellationToken ct = default);

    // Bar chart endpoints
    Task<TopTreeByMetricResponse> GetTopTreesByMetricChartAsync(Guid plantationId, string metricType, int days = 30, int limit = 10, CancellationToken ct = default);
    Task<MonthlyHarvestComparisonResponse> GetMonthlyHarvestComparisonChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<MonthlyTreeActivityResponse> GetTreeActivityTrendChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<TreesByZoneComparisonResponse> GetTreesByZoneChartAsync(Guid plantationId, int days = 30, CancellationToken ct = default);
    Task<WeeklyHarvestPerformanceResponse> GetWeeklyHarvestPerformanceChartAsync(Guid plantationId, int weeks = 12, CancellationToken ct = default);

    // Histogram endpoints
    Task<TreeYieldDistributionHistogramResponse> GetTreeYieldDistributionHistogramAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, int binCount = 10, CancellationToken ct = default);
    Task<AirTemperatureDistributionHistogramResponse> GetAirTemperatureDistributionHistogramAsync(Guid plantationId, int days = 30, int binCount = 15, CancellationToken ct = default);
    Task<SoilTemperatureDistributionHistogramResponse> GetSoilTemperatureDistributionHistogramAsync(Guid plantationId, int days = 30, int binCount = 15, CancellationToken ct = default);
    Task<SoilMoistureDistributionHistogramResponse> GetSoilMoistureDistributionHistogramAsync(Guid plantationId, int days = 30, int binCount = 15, CancellationToken ct = default);
    Task<TreeAgeDistributionHistogramResponse> GetTreeAgeDistributionHistogramAsync(Guid plantationId, int binCount = 10, CancellationToken ct = default);

    // Area chart endpoints
    Task<PlantationCumulativeHarvestYieldResponse> GetCumulativeHarvestYieldAreaChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "weekly", CancellationToken ct = default);
    Task<PlantationCumulativeHarvestCountResponse> GetCumulativeHarvestCountAreaChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "weekly", CancellationToken ct = default);
    Task<TreeMonitoringAdoptionResponse> GetTreeMonitoringAdoptionAreaChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "weekly", CancellationToken ct = default);
    Task<CumulativeMetricReadingsResponse> GetCumulativeMetricReadingsAreaChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<StackedMetricsByTypeResponse> GetStackedMetricsByTypeAreaChartAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
}


