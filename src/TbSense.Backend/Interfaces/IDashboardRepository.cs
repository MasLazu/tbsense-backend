using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Interfaces;

/// <summary>
/// Repository interface for global dashboard data with database-side aggregation
/// </summary>
public interface IDashboardRepository
{
    // Summary endpoints
    Task<PlantationsSummaryResponse> GetPlantationsSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<TreesSummaryResponse> GetTreesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<LandAreaSummaryResponse> GetLandAreaSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<HarvestSummaryResponse> GetHarvestSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Environmental data
    Task<EnvironmentalMetricValue> GetEnvironmentalAveragesAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Timeseries data
    Task<List<EnvironmentalTimeseriesDataPoint>> GetEnvironmentalTimeseriesAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<List<HarvestTimeseriesDataPoint>> GetHarvestTimeseriesAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);
    Task<List<PlantationGrowthDataPoint>> GetPlantationGrowthTimeseriesAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);

    // Chart data
    Task<List<PlantationDistributionItem>> GetPlantationsByTreesChartAsync(CancellationToken ct = default);
    Task<List<PlantationLandDistributionItem>> GetPlantationsByLandAreaChartAsync(CancellationToken ct = default);
    Task<List<HarvestDistributionItem>> GetHarvestByPlantationChartAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<TreeActivityStatusItem>> GetTreeActivityStatusChartAsync(CancellationToken ct = default);

    // Bar chart data
    Task<List<PlantationYieldComparisonItem>> GetTopPlantationsByYieldChartAsync(DateTime? startDate = null, DateTime? endDate = null, int limit = 10, CancellationToken ct = default);
    Task<List<PlantationAvgHarvestComparisonItem>> GetTopPlantationsByAvgHarvestChartAsync(DateTime? startDate = null, DateTime? endDate = null, int limit = 10, CancellationToken ct = default);
    Task<List<PlantationTreeCountComparisonItem>> GetTreeCountByPlantationChartAsync(int limit = 10, CancellationToken ct = default);
    Task<List<PlantationActivityComparisonItem>> GetTreeActivityByPlantationChartAsync(CancellationToken ct = default);
    Task<List<PlantationHarvestFrequencyItem>> GetHarvestFrequencyByPlantationChartAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Histogram data
    Task<List<double>> GetYieldDistributionDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetPlantationSizeDistributionDataAsync(CancellationToken ct = default);
    Task<List<double>> GetTreeDensityDistributionDataAsync(CancellationToken ct = default);
    Task<List<double>> GetHarvestFrequencyDistributionDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<double>> GetAvgHarvestSizeDistributionDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Area chart data
    Task<List<(DateTime Date, double Value)>> GetCumulativeYieldDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, int Count)>> GetCumulativeHarvestCountDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, int Count)>> GetPlantationGrowthDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, int Count)>> GetTreePopulationGrowthDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, int Count)>> GetCumulativeActiveTreesDataAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<List<(DateTime Date, string PlantationName, double Yield)>> GetStackedYieldByPlantationDataAsync(DateTime? startDate = null, DateTime? endDate = null, int limit = 5, CancellationToken ct = default);
}


