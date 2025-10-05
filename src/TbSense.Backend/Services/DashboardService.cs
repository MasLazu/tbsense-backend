using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Interfaces;
using TbSense.Backend.Utils;

namespace TbSense.Backend.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PlantationsSummaryResponse> GetPlantationsSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        return await _repository.GetPlantationsSummaryAsync(startDate, endDate, ct);
    }

    public async Task<TreesSummaryResponse> GetTreesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        return await _repository.GetTreesSummaryAsync(startDate, endDate, ct);
    }

    public async Task<LandAreaSummaryResponse> GetLandAreaSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        return await _repository.GetLandAreaSummaryAsync(startDate, endDate, ct);
    }

    public async Task<HarvestSummaryResponse> GetHarvestSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        return await _repository.GetHarvestSummaryAsync(startDate, endDate, ct);
    }

    public async Task<EnvironmentalAveragesResponse> GetEnvironmentalAveragesAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        EnvironmentalMetricValue metrics = await _repository.GetEnvironmentalAveragesAsync(startDate, endDate, ct);
        return new EnvironmentalAveragesResponse(metrics);
    }

    public async Task<EnvironmentalTimeseriesResponse> GetEnvironmentalTimeseriesAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<EnvironmentalTimeseriesDataPoint> dataPoints = await _repository.GetEnvironmentalTimeseriesAsync(startDate, endDate, interval, ct);
        return new EnvironmentalTimeseriesResponse(dataPoints);
    }

    public async Task<HarvestTimeseriesResponse> GetHarvestTimeseriesAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "monthly",
        CancellationToken ct = default)
    {
        List<HarvestTimeseriesDataPoint> dataPoints = await _repository.GetHarvestTimeseriesAsync(startDate, endDate, interval, ct);
        return new HarvestTimeseriesResponse(dataPoints);
    }

    public async Task<PlantationGrowthTimeseriesResponse> GetPlantationGrowthTimeseriesAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "monthly",
        CancellationToken ct = default)
    {
        List<PlantationGrowthDataPoint> dataPoints = await _repository.GetPlantationGrowthTimeseriesAsync(startDate, endDate, interval, ct);
        return new PlantationGrowthTimeseriesResponse(dataPoints);
    }

    public async Task<PlantationDistributionResponse> GetPlantationsByTreesChartAsync(CancellationToken ct = default)
    {
        List<PlantationDistributionItem> items = await _repository.GetPlantationsByTreesChartAsync(ct);
        int totalTrees = items.Sum(i => i.TreeCount);
        return new PlantationDistributionResponse(items, totalTrees);
    }

    public async Task<PlantationLandDistributionResponse> GetPlantationsByLandAreaChartAsync(CancellationToken ct = default)
    {
        List<PlantationLandDistributionItem> items = await _repository.GetPlantationsByLandAreaChartAsync(ct);
        double totalLandArea = items.Sum(i => i.LandAreaHectares);
        return new PlantationLandDistributionResponse(items, totalLandArea);
    }

    public async Task<HarvestDistributionResponse> GetHarvestByPlantationChartAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        List<HarvestDistributionItem> items = await _repository.GetHarvestByPlantationChartAsync(startDate, endDate, ct);
        double totalYieldKg = items.Sum(i => i.TotalYieldKg);
        int totalHarvests = items.Sum(i => i.HarvestCount);
        return new HarvestDistributionResponse(items, totalYieldKg, totalHarvests);
    }

    public async Task<TreeActivityStatusResponse> GetTreeActivityStatusChartAsync(CancellationToken ct = default)
    {
        List<TreeActivityStatusItem> items = await _repository.GetTreeActivityStatusChartAsync(ct);
        int totalTrees = items.Sum(i => i.TreeCount);
        return new TreeActivityStatusResponse(items, totalTrees);
    }

    public async Task<PlantationYieldComparisonResponse> GetTopPlantationsByYieldChartAsync(DateTime? startDate = null, DateTime? endDate = null, int limit = 10, CancellationToken ct = default)
    {
        List<PlantationYieldComparisonItem> items = await _repository.GetTopPlantationsByYieldChartAsync(startDate, endDate, limit, ct);
        double totalYield = items.Sum(i => i.TotalYieldKg);
        int totalHarvests = items.Sum(i => i.HarvestCount);
        return new PlantationYieldComparisonResponse(items, totalYield, totalHarvests);
    }

    public async Task<PlantationAvgHarvestComparisonResponse> GetTopPlantationsByAvgHarvestChartAsync(DateTime? startDate = null, DateTime? endDate = null, int limit = 10, CancellationToken ct = default)
    {
        List<PlantationAvgHarvestComparisonItem> items = await _repository.GetTopPlantationsByAvgHarvestChartAsync(startDate, endDate, limit, ct);
        return new PlantationAvgHarvestComparisonResponse(items, items.Count);
    }

    public async Task<PlantationTreeCountComparisonResponse> GetTreeCountByPlantationChartAsync(int limit = 10, CancellationToken ct = default)
    {
        List<PlantationTreeCountComparisonItem> items = await _repository.GetTreeCountByPlantationChartAsync(limit, ct);
        int totalTrees = items.Sum(i => i.TreeCount);
        return new PlantationTreeCountComparisonResponse(items, totalTrees);
    }

    public async Task<PlantationActivityComparisonResponse> GetTreeActivityByPlantationChartAsync(CancellationToken ct = default)
    {
        List<PlantationActivityComparisonItem> items = await _repository.GetTreeActivityByPlantationChartAsync(ct);
        int totalActiveTrees = items.Sum(i => i.ActiveTrees);
        int totalInactiveTrees = items.Sum(i => i.InactiveTrees);
        return new PlantationActivityComparisonResponse(items, totalActiveTrees, totalInactiveTrees);
    }

    public async Task<PlantationHarvestFrequencyResponse> GetHarvestFrequencyByPlantationChartAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        List<PlantationHarvestFrequencyItem> items = await _repository.GetHarvestFrequencyByPlantationChartAsync(startDate, endDate, ct);
        int totalHarvests = items.Sum(i => i.HarvestCount);
        return new PlantationHarvestFrequencyResponse(items, totalHarvests);
    }

    // Histogram endpoints
    public async Task<YieldDistributionHistogramResponse> GetYieldDistributionHistogramAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int binCount = 10,
        CancellationToken ct = default)
    {
        List<double> data = await _repository.GetYieldDistributionDataAsync(startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.##", "kg");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new YieldDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<PlantationSizeDistributionHistogramResponse> GetPlantationSizeDistributionHistogramAsync(
        int binCount = 10,
        CancellationToken ct = default)
    {
        List<double> data = await _repository.GetPlantationSizeDistributionDataAsync(ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.##", "hectares");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new PlantationSizeDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<TreeDensityDistributionHistogramResponse> GetTreeDensityDistributionHistogramAsync(
        int binCount = 10,
        CancellationToken ct = default)
    {
        List<double> data = await _repository.GetTreeDensityDistributionDataAsync(ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.##", "trees/hectare");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new TreeDensityDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<HarvestFrequencyDistributionHistogramResponse> GetHarvestFrequencyDistributionHistogramAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int binCount = 10,
        CancellationToken ct = default)
    {
        List<double> data = await _repository.GetHarvestFrequencyDistributionDataAsync(startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0", "harvests");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new HarvestFrequencyDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    public async Task<AvgHarvestSizeDistributionHistogramResponse> GetAvgHarvestSizeDistributionHistogramAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int binCount = 10,
        CancellationToken ct = default)
    {
        List<double> data = await _repository.GetAvgHarvestSizeDistributionDataAsync(startDate, endDate, ct);
        List<HistogramBinItem> bins = HistogramHelper.CreateHistogramBins(data, binCount, "0.##", "kg/harvest");
        (double min, double max, double average, double median) = HistogramHelper.CalculateStatistics(data);
        return new AvgHarvestSizeDistributionHistogramResponse(bins, data.Count, min, max, average, median);
    }

    // Area chart methods
    public async Task<CumulativeYieldResponse> GetCumulativeYieldAreaChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<(DateTime Date, double Value)> data = await _repository.GetCumulativeYieldDataAsync(startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateSimpleAreaChart(data, interval);
        double totalYield = chartData.LastOrDefault()?.CumulativeValue ?? 0;
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new CumulativeYieldResponse(chartData, totalYield, avgIncrease, interval);
    }

    public async Task<CumulativeHarvestCountResponse> GetCumulativeHarvestCountAreaChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<(DateTime Date, int Count)> data = await _repository.GetCumulativeHarvestCountDataAsync(startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalHarvests = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new CumulativeHarvestCountResponse(chartData, totalHarvests, avgIncrease, interval);
    }

    public async Task<PlantationGrowthResponse> GetPlantationGrowthAreaChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<(DateTime Date, int Count)> data = await _repository.GetPlantationGrowthDataAsync(startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalPlantations = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new PlantationGrowthResponse(chartData, totalPlantations, avgIncrease, interval);
    }

    public async Task<TreePopulationGrowthResponse> GetTreePopulationGrowthAreaChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<(DateTime Date, int Count)> data = await _repository.GetTreePopulationGrowthDataAsync(startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalTrees = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new TreePopulationGrowthResponse(chartData, totalTrees, avgIncrease, interval);
    }

    public async Task<CumulativeActiveTreesResponse> GetCumulativeActiveTreesAreaChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        List<(DateTime Date, int Count)> data = await _repository.GetCumulativeActiveTreesDataAsync(startDate, endDate, ct);
        List<AreaChartDataPoint> chartData = AreaChartHelper.CreateCountAreaChart(data, interval);
        int totalActiveTrees = (int)(chartData.LastOrDefault()?.CumulativeValue ?? 0);
        double avgIncrease = AreaChartHelper.CalculateAverageIncrease(chartData, interval);
        return new CumulativeActiveTreesResponse(chartData, totalActiveTrees, avgIncrease, interval);
    }

    public async Task<StackedYieldByPlantationResponse> GetStackedYieldByPlantationAreaChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "monthly",
        int limit = 5,
        CancellationToken ct = default)
    {
        List<(DateTime Date, string PlantationName, double Yield)> data = await _repository.GetStackedYieldByPlantationDataAsync(startDate, endDate, limit, ct);
        List<StackedAreaChartDataPoint> chartData = AreaChartHelper.CreateStackedAreaChart(data, interval);

        // Calculate totals by plantation
        var totalsByPlantation = data
            .GroupBy(d => d.PlantationName)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Yield));

        double grandTotal = chartData.LastOrDefault()?.TotalCumulative ?? 0;
        int plantationCount = totalsByPlantation.Count;

        return new StackedYieldByPlantationResponse(chartData, totalsByPlantation, grandTotal, interval, plantationCount);
    }
}




