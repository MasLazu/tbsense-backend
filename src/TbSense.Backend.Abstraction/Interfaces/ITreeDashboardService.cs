using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface ITreeDashboardService
{
    Task<TreeBasicInfoResponse?> GetTreeBasicInfoAsync(Guid treeId, CancellationToken ct = default);
    Task<TreeCurrentMetricsResponse?> GetTreeCurrentMetricsAsync(Guid treeId, CancellationToken ct = default);
    Task<TreeEnvironmentalAveragesResponse> GetTreeEnvironmentalAveragesAsync(Guid treeId, int days = 30, CancellationToken ct = default);
    Task<TreeEnvironmentalTimeseriesResponse> GetTreeEnvironmentalTimeseriesAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);

    // Chart endpoints
    Task<ReadingDistributionResponse> GetTreeReadingDistributionChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<MetricRangesResponse> GetTreeMetricRangesChartAsync(Guid treeId, string metricType = "airTemperature", int days = 30, CancellationToken ct = default);

    // Bar chart endpoints
    Task<DailyMetricComparisonResponse> GetDailyMetricsComparisonChartAsync(Guid treeId, int days = 7, CancellationToken ct = default);
    Task<HourlyAverageComparisonResponse> GetHourlyAverageComparisonChartAsync(Guid treeId, int days = 30, CancellationToken ct = default);
    Task<WeeklyMetricComparisonResponse> GetWeeklyMetricsComparisonChartAsync(Guid treeId, int weeks = 8, CancellationToken ct = default);
    Task<MetricRangeComparisonResponse> GetMetricRangeComparisonChartAsync(Guid treeId, int days = 30, CancellationToken ct = default);

    // Histogram endpoints
    Task<AirTemperatureDistributionHistogramResponse> GetAirTemperatureDistributionHistogramAsync(Guid treeId, int days = 30, int binCount = 15, CancellationToken ct = default);
    Task<SoilTemperatureDistributionHistogramResponse> GetSoilTemperatureDistributionHistogramAsync(Guid treeId, int days = 30, int binCount = 15, CancellationToken ct = default);
    Task<SoilMoistureDistributionHistogramResponse> GetSoilMoistureDistributionHistogramAsync(Guid treeId, int days = 30, int binCount = 15, CancellationToken ct = default);

    // Area chart endpoints
    Task<TreeCumulativeMetricReadingsResponse> GetCumulativeMetricReadingsAreaChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<TreeStackedMetricsByTypeResponse> GetStackedMetricsByTypeAreaChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<CumulativeTemperatureExposureResponse> GetCumulativeTemperatureExposureAreaChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", double threshold = 30.0, CancellationToken ct = default);
    Task<CumulativeMoistureDeficitResponse> GetCumulativeMoistureDeficitAreaChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", double threshold = 30.0, CancellationToken ct = default);
}


