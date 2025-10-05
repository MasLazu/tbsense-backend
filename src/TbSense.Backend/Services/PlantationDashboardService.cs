using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Interfaces;
using TbSense.Backend.Utils;

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
        return await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
    }

    public async Task<PlantationTreesSummaryResponse> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default)
    {
        return await _repository.GetPlantationTreesSummaryAsync(plantationId, ct);
    }

    public async Task<PlantationHarvestSummaryResponse> GetPlantationHarvestSummaryAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        return await _repository.GetPlantationHarvestSummaryAsync(plantationId, startDate, endDate, ct);
    }

    public async Task<PlantationRankingResponse?> GetPlantationRankingAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        return await _repository.GetPlantationRankingAsync(plantationId, startDate, endDate, ct);
    }

    public async Task<PlantationCurrentMetricsResponse?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default)
    {
        return await _repository.GetPlantationCurrentMetricsAsync(plantationId, ct);
    }

    public async Task<PlantationEnvironmentalAveragesResponse> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days = 30, CancellationToken ct = default)
    {
        PlantationEnvironmentalMetrics metrics = await _repository.GetPlantationEnvironmentalAveragesAsync(plantationId, days, ct);
        return new PlantationEnvironmentalAveragesResponse(metrics);
    }

    public async Task<PlantationEnvironmentalTimeseriesResponse> GetPlantationEnvironmentalTimeseriesAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<PlantationEnvironmentalTimeseriesDataPoint> dataPoints = await _repository.GetPlantationEnvironmentalTimeseriesAsync(plantationId, startDate, endDate, interval, ct);
        return new PlantationEnvironmentalTimeseriesResponse(dataPoints);
    }

    public async Task<PlantationHarvestTimeseriesResponse> GetPlantationHarvestTimeseriesAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "monthly",
        CancellationToken ct = default)
    {
        List<PlantationHarvestTimeseriesDataPoint> dataPoints = await _repository.GetPlantationHarvestTimeseriesAsync(plantationId, startDate, endDate, interval, ct);
        return new PlantationHarvestTimeseriesResponse(dataPoints);
    }

    public async Task<PlantationTreeGrowthTimeseriesResponse> GetPlantationTreeGrowthTimeseriesAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "monthly",
        CancellationToken ct = default)
    {
        List<PlantationTreeGrowthDataPoint> dataPoints = await _repository.GetPlantationTreeGrowthTimeseriesAsync(plantationId, startDate, endDate, interval, ct);
        return new PlantationTreeGrowthTimeseriesResponse(dataPoints);
    }

    public async Task<PlantationTreeActivityResponse> GetPlantationTreeActivityChartAsync(Guid plantationId, CancellationToken ct = default)
    {
        List<PlantationTreeActivityItem> items = await _repository.GetPlantationTreeActivityChartAsync(plantationId, ct);
        int totalTrees = items.Sum(i => i.TreeCount);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new PlantationTreeActivityResponse(plantationId, plantationName, items, totalTrees);
    }

    public async Task<MonthlyHarvestDistributionResponse> GetPlantationHarvestByMonthChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        List<MonthlyHarvestDistributionItem> items = await _repository.GetPlantationHarvestByMonthChartAsync(plantationId, startDate, endDate, ct);
        double totalYieldKg = items.Sum(i => i.TotalYieldKg);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new MonthlyHarvestDistributionResponse(plantationId, plantationName, items, totalYieldKg);
    }

    public async Task<EnvironmentalZonesResponse> GetPlantationEnvironmentalZonesChartAsync(
        Guid plantationId,
        int days = 30,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<EnvironmentalZoneItem> items = await _repository.GetPlantationEnvironmentalZonesChartAsync(plantationId, startDate, endDate, ct);
        int totalTrees = items.Sum(i => i.TreeCount);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new EnvironmentalZonesResponse(plantationId, plantationName, items, totalTrees);
    }

    public async Task<TopTreeByMetricResponse> GetTopTreesByMetricChartAsync(
        Guid plantationId,
        string metricType,
        int days = 30,
        int limit = 10,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<TopTreeByMetricItem> items = await _repository.GetTopTreesByMetricChartAsync(plantationId, metricType, startDate, endDate, limit, ct);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new TopTreeByMetricResponse(plantationId, plantationName, metricType, items, items.Count);
    }

    public async Task<MonthlyHarvestComparisonResponse> GetMonthlyHarvestComparisonChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        List<MonthlyHarvestComparisonItem> items = await _repository.GetMonthlyHarvestComparisonChartAsync(plantationId, startDate, endDate, ct);
        double totalYield = items.Sum(i => i.TotalYieldKg);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new MonthlyHarvestComparisonResponse(plantationId, plantationName, items, totalYield);
    }

    public async Task<MonthlyTreeActivityResponse> GetTreeActivityTrendChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        List<MonthlyTreeActivityItem> items = await _repository.GetTreeActivityTrendChartAsync(plantationId, startDate, endDate, ct);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new MonthlyTreeActivityResponse(plantationId, plantationName, items);
    }

    public async Task<TreesByZoneComparisonResponse> GetTreesByZoneChartAsync(
        Guid plantationId,
        int days = 30,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<TreesByZoneComparisonItem> items = await _repository.GetTreesByZoneChartAsync(plantationId, startDate, endDate, ct);
        int totalTrees = items.Sum(i => i.TreeCount);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new TreesByZoneComparisonResponse(plantationId, plantationName, items, totalTrees);
    }

    public async Task<WeeklyHarvestPerformanceResponse> GetWeeklyHarvestPerformanceChartAsync(
        Guid plantationId,
        int weeks = 12,
        CancellationToken ct = default)
    {
        List<WeeklyHarvestPerformanceItem> items = await _repository.GetWeeklyHarvestPerformanceChartAsync(plantationId, weeks, ct);
        double totalYield = items.Sum(i => i.TotalYieldKg);

        PlantationBasicSummaryResponse? plantation = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = plantation?.Name ?? "Unknown";

        return new WeeklyHarvestPerformanceResponse(plantationId, plantationName, items, totalYield);
    }

    // Histogram endpoints
    public async Task<TreeYieldDistributionHistogramResponse> GetTreeYieldDistributionHistogramAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int binCount = 10,
        CancellationToken ct = default)
    {
        List<double> data = await _repository.GetTreeYieldDistributionDataAsync(plantationId, startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.##", "kg");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new TreeYieldDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<AirTemperatureDistributionHistogramResponse> GetAirTemperatureDistributionHistogramAsync(
        Guid plantationId,
        int days = 30,
        int binCount = 15,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<double> data = await _repository.GetAirTemperatureDistributionDataAsync(plantationId, startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.#", "°C");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new AirTemperatureDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<SoilTemperatureDistributionHistogramResponse> GetSoilTemperatureDistributionHistogramAsync(
        Guid plantationId,
        int days = 30,
        int binCount = 15,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<double> data = await _repository.GetSoilTemperatureDistributionDataAsync(plantationId, startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.#", "°C");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new SoilTemperatureDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<SoilMoistureDistributionHistogramResponse> GetSoilMoistureDistributionHistogramAsync(
        Guid plantationId,
        int days = 30,
        int binCount = 15,
        CancellationToken ct = default)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        List<double> data = await _repository.GetSoilMoistureDistributionDataAsync(plantationId, startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.#", "%");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new SoilMoistureDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<TreeAgeDistributionHistogramResponse> GetTreeAgeDistributionHistogramAsync(
        Guid plantationId,
        int binCount = 10,
        CancellationToken ct = default)
    {
        List<double> data = await _repository.GetTreeAgeDistributionDataAsync(plantationId, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.#", "years");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new TreeAgeDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    // Area chart methods
    public async Task<PlantationCumulativeHarvestYieldResponse> GetCumulativeHarvestYieldAreaChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "weekly",
        CancellationToken ct = default)
    {
        PlantationBasicSummaryResponse? basic = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = basic?.Name ?? "Unknown";

        List<(DateTime Date, double Yield)> data = await _repository.GetCumulativeHarvestYieldDataAsync(plantationId, startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateSimpleAreaChart(data, interval);
        double totalYield = chartData.LastOrDefault()?.CumulativeValue ?? 0;
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new PlantationCumulativeHarvestYieldResponse(plantationId, plantationName, chartData, totalYield, avgIncrease, interval);
    }

    public async Task<PlantationCumulativeHarvestCountResponse> GetCumulativeHarvestCountAreaChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "weekly",
        CancellationToken ct = default)
    {
        PlantationBasicSummaryResponse? basic = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = basic?.Name ?? "Unknown";

        List<(DateTime Date, int Count)> data = await _repository.GetCumulativeHarvestCountDataAsync(plantationId, startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalHarvests = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new PlantationCumulativeHarvestCountResponse(plantationId, plantationName, chartData, totalHarvests, avgIncrease, interval);
    }

    public async Task<TreeMonitoringAdoptionResponse> GetTreeMonitoringAdoptionAreaChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "weekly",
        CancellationToken ct = default)
    {
        PlantationBasicSummaryResponse? basic = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = basic?.Name ?? "Unknown";

        List<(DateTime Date, int Count)> data = await _repository.GetTreeMonitoringAdoptionDataAsync(plantationId, startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalTreesMonitored = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new TreeMonitoringAdoptionResponse(plantationId, plantationName, chartData, totalTreesMonitored, avgIncrease, interval);
    }

    public async Task<CumulativeMetricReadingsResponse> GetCumulativeMetricReadingsAreaChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        PlantationBasicSummaryResponse? basic = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = basic?.Name ?? "Unknown";

        List<(DateTime Date, int Count)> data = await _repository.GetCumulativeMetricReadingsDataAsync(plantationId, startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalReadings = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new CumulativeMetricReadingsResponse(plantationId, plantationName, chartData, totalReadings, avgIncrease, interval);
    }

    public async Task<StackedMetricsByTypeResponse> GetStackedMetricsByTypeAreaChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        PlantationBasicSummaryResponse? basic = await _repository.GetPlantationBasicSummaryAsync(plantationId, ct);
        string plantationName = basic?.Name ?? "Unknown";

        List<(DateTime Date, string MetricType, double Value)> data = await _repository.GetStackedMetricsByTypeDataAsync(plantationId, startDate, endDate, ct);
        List<StackedAreaChartDataPoint> chartData = AreaChartHelper.CreateStackedAreaChart(data, interval);

        // Calculate totals by metric type
        var totalsByType = data
            .GroupBy(d => d.MetricType)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));

        double grandTotal = chartData.LastOrDefault()?.TotalCumulative ?? 0;

        return new StackedMetricsByTypeResponse(plantationId, plantationName, chartData, totalsByType, grandTotal, interval);
    }
}


