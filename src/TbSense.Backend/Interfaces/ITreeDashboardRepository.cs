using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Interfaces;

/// <summary>
/// Repository interface for tree-specific dashboard data
/// </summary>
public interface ITreeDashboardRepository
{
    Task<TreeBasicInfoResponse?> GetTreeBasicInfoAsync(Guid treeId, CancellationToken ct = default);
    Task<TreeCurrentMetricsResponse?> GetTreeCurrentMetricsAsync(Guid treeId, CancellationToken ct = default);
    Task<TreeEnvironmentalAveragesResponse> GetTreeEnvironmentalAveragesAsync(Guid treeId, int days, CancellationToken ct = default);
    Task<List<TreeEnvironmentalTimeseriesDataPoint>> GetTreeEnvironmentalTimeseriesAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);

    // Chart data
    Task<List<ReadingDistributionItem>> GetTreeReadingDistributionChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<MetricRangeItem>> GetTreeMetricRangesChartAsync(Guid treeId, string metricType, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Bar chart data
    Task<List<DailyMetricComparisonItem>> GetDailyMetricsComparisonChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<HourlyAverageComparisonItem>> GetHourlyAverageComparisonChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<WeeklyMetricComparisonItem>> GetWeeklyMetricsComparisonChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<MetricRangeComparisonItem>> GetMetricRangeComparisonChartAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Histogram data
    Task<List<double>> GetAirTemperatureDistributionDataAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetSoilTemperatureDistributionDataAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetSoilMoistureDistributionDataAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Area chart data
    Task<List<(DateTime Date, int Count)>> GetCumulativeMetricReadingsDataAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, string MetricType, double Value)>> GetStackedMetricsByTypeDataAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, double Temperature)>> GetCumulativeTemperatureExposureDataAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, double threshold = 30.0, CancellationToken ct = default);
    Task<List<(DateTime Date, double Moisture)>> GetCumulativeMoistureDeficitDataAsync(Guid treeId, DateTime? startDate = null, DateTime? endDate = null, double threshold = 30.0, CancellationToken ct = default);
}


