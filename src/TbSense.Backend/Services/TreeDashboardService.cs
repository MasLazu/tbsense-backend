using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Interfaces;
using TbSense.Backend.Utils;

namespace TbSense.Backend.Services;

public class TreeDashboardService : ITreeDashboardService
{
    private readonly ITreeDashboardRepository _repository;

    public TreeDashboardService(ITreeDashboardRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<TreeBasicInfoResponse?> GetTreeBasicInfoAsync(Guid treeId, CancellationToken ct = default)
    {
        return await _repository.GetTreeBasicInfoAsync(treeId, ct);
    }

    public async Task<TreeCurrentMetricsResponse?> GetTreeCurrentMetricsAsync(Guid treeId, CancellationToken ct = default)
    {
        return await _repository.GetTreeCurrentMetricsAsync(treeId, ct);
    }

    public async Task<TreeEnvironmentalAveragesResponse> GetTreeEnvironmentalAveragesAsync(
        Guid treeId,
        int days = 30,
        CancellationToken ct = default)
    {
        return await _repository.GetTreeEnvironmentalAveragesAsync(treeId, days, ct);
    }

    public async Task<TreeEnvironmentalTimeseriesResponse> GetTreeEnvironmentalTimeseriesAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<TreeEnvironmentalTimeseriesDataPoint> dataPoints = await _repository.GetTreeEnvironmentalTimeseriesAsync(treeId, startDate, endDate, interval, ct);
        return new TreeEnvironmentalTimeseriesResponse(dataPoints);
    }

    public async Task<ReadingDistributionResponse> GetTreeReadingDistributionChartAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        List<ReadingDistributionItem> items = await _repository.GetTreeReadingDistributionChartAsync(treeId, startDate, endDate, ct);
        int totalReadings = items.Sum(i => i.ReadingCount);
        return new ReadingDistributionResponse(treeId, items, totalReadings);
    }

    public async Task<MetricRangesResponse> GetTreeMetricRangesChartAsync(
        Guid treeId,
        string metricType = "airTemperature",
        int days = 30,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<MetricRangeItem> items = await _repository.GetTreeMetricRangesChartAsync(treeId, metricType, startDate, endDate, ct);
        int totalReadings = items.Sum(i => i.ReadingCount);
        return new MetricRangesResponse(treeId, metricType, items, totalReadings);
    }

    public async Task<DailyMetricComparisonResponse> GetDailyMetricsComparisonChartAsync(
        Guid treeId,
        int days = 7,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<DailyMetricComparisonItem> items = await _repository.GetDailyMetricsComparisonChartAsync(treeId, startDate, endDate, ct);
        return new DailyMetricComparisonResponse(treeId, items);
    }

    public async Task<HourlyAverageComparisonResponse> GetHourlyAverageComparisonChartAsync(
        Guid treeId,
        int days = 30,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<HourlyAverageComparisonItem> items = await _repository.GetHourlyAverageComparisonChartAsync(treeId, startDate, endDate, ct);
        return new HourlyAverageComparisonResponse(treeId, items);
    }

    public async Task<WeeklyMetricComparisonResponse> GetWeeklyMetricsComparisonChartAsync(
        Guid treeId,
        int weeks = 8,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-(weeks * 7));

        List<WeeklyMetricComparisonItem> items = await _repository.GetWeeklyMetricsComparisonChartAsync(treeId, startDate, endDate, ct);
        return new WeeklyMetricComparisonResponse(treeId, items);
    }

    public async Task<MetricRangeComparisonResponse> GetMetricRangeComparisonChartAsync(
        Guid treeId,
        int days = 30,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<MetricRangeComparisonItem> items = await _repository.GetMetricRangeComparisonChartAsync(treeId, startDate, endDate, ct);
        return new MetricRangeComparisonResponse(treeId, items, days);
    }

    // Histogram endpoints
    public async Task<AirTemperatureDistributionHistogramResponse> GetAirTemperatureDistributionHistogramAsync(
        Guid treeId,
        int days = 30,
        int binCount = 15,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<double> data = await _repository.GetAirTemperatureDistributionDataAsync(treeId, startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.#", "°C");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new AirTemperatureDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<SoilTemperatureDistributionHistogramResponse> GetSoilTemperatureDistributionHistogramAsync(
        Guid treeId,
        int days = 30,
        int binCount = 15,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<double> data = await _repository.GetSoilTemperatureDistributionDataAsync(treeId, startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.#", "°C");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new SoilTemperatureDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<SoilMoistureDistributionHistogramResponse> GetSoilMoistureDistributionHistogramAsync(
        Guid treeId,
        int days = 30,
        int binCount = 15,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<double> data = await _repository.GetSoilMoistureDistributionDataAsync(treeId, startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.#", "%");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new SoilMoistureDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    // Area chart methods
    public async Task<TreeCumulativeMetricReadingsResponse> GetCumulativeMetricReadingsAreaChartAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<(DateTime Date, int Count)> data = await _repository.GetCumulativeMetricReadingsDataAsync(treeId, startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalReadings = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new TreeCumulativeMetricReadingsResponse(treeId, chartData, totalReadings, avgIncrease, interval);
    }

    public async Task<TreeStackedMetricsByTypeResponse> GetStackedMetricsByTypeAreaChartAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<(DateTime Date, string MetricType, double Value)> data = await _repository.GetStackedMetricsByTypeDataAsync(treeId, startDate, endDate, ct);
        List<StackedAreaChartDataPoint> chartData = AreaChartHelper.CreateStackedAreaChart(data, interval);

        // Calculate totals by metric type
        var totalsByType = data
            .GroupBy(d => d.MetricType)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));

        double grandTotal = chartData.LastOrDefault()?.TotalCumulative ?? 0;

        return new TreeStackedMetricsByTypeResponse(treeId, chartData, totalsByType, grandTotal, interval);
    }

    public async Task<CumulativeTemperatureExposureResponse> GetCumulativeTemperatureExposureAreaChartAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        double threshold = 30.0,
        CancellationToken ct = default)
    {
        List<(DateTime Date, double Temperature)> data = await _repository.GetCumulativeTemperatureExposureDataAsync(treeId, startDate, endDate, threshold, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateSimpleAreaChart(data, interval);
        double totalExposure = chartData.LastOrDefault()?.CumulativeValue ?? 0;
        return new CumulativeTemperatureExposureResponse(treeId, chartData, totalExposure, threshold, "Air Temperature", interval);
    }

    public async Task<CumulativeMoistureDeficitResponse> GetCumulativeMoistureDeficitAreaChartAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        double threshold = 30.0,
        CancellationToken ct = default)
    {
        List<(DateTime Date, double Moisture)> data = await _repository.GetCumulativeMoistureDeficitDataAsync(treeId, startDate, endDate, threshold, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateSimpleAreaChart(data, interval);
        double totalDeficit = chartData.LastOrDefault()?.CumulativeValue ?? 0;
        return new CumulativeMoistureDeficitResponse(treeId, chartData, totalDeficit, threshold, interval);
    }
}


