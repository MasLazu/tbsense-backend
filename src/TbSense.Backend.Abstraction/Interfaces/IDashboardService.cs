using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Abstraction.Interfaces;

public interface IDashboardService
{
    Task<PlantationsSummaryResponse> GetPlantationsSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<TreesSummaryResponse> GetTreesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<LandAreaSummaryResponse> GetLandAreaSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<HarvestSummaryResponse> GetHarvestSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<EnvironmentalAveragesResponse> GetEnvironmentalAveragesAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<EnvironmentalTimeseriesResponse> GetEnvironmentalTimeseriesAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<HarvestTimeseriesResponse> GetHarvestTimeseriesAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);
    Task<PlantationGrowthTimeseriesResponse> GetPlantationGrowthTimeseriesAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);

    // Chart endpoints
    Task<PlantationDistributionResponse> GetPlantationsByTreesChartAsync(CancellationToken ct = default);
    Task<PlantationLandDistributionResponse> GetPlantationsByLandAreaChartAsync(CancellationToken ct = default);
    Task<HarvestDistributionResponse> GetHarvestByPlantationChartAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);
    Task<TreeActivityStatusResponse> GetTreeActivityStatusChartAsync(CancellationToken ct = default);

    // Bar chart endpoints
    Task<PlantationYieldComparisonResponse> GetTopPlantationsByYieldChartAsync(DateTime? startDate = null, DateTime? endDate = null, int limit = 10, CancellationToken ct = default);
    Task<PlantationAvgHarvestComparisonResponse> GetTopPlantationsByAvgHarvestChartAsync(DateTime? startDate = null, DateTime? endDate = null, int limit = 10, CancellationToken ct = default);
    Task<PlantationTreeCountComparisonResponse> GetTreeCountByPlantationChartAsync(int limit = 10, CancellationToken ct = default);
    Task<PlantationActivityComparisonResponse> GetTreeActivityByPlantationChartAsync(CancellationToken ct = default);
    Task<PlantationHarvestFrequencyResponse> GetHarvestFrequencyByPlantationChartAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default);

    // Histogram endpoints
    Task<YieldDistributionHistogramResponse> GetYieldDistributionHistogramAsync(DateTime? startDate = null, DateTime? endDate = null, int binCount = 10, CancellationToken ct = default);
    Task<PlantationSizeDistributionHistogramResponse> GetPlantationSizeDistributionHistogramAsync(int binCount = 10, CancellationToken ct = default);
    Task<TreeDensityDistributionHistogramResponse> GetTreeDensityDistributionHistogramAsync(int binCount = 10, CancellationToken ct = default);
    Task<HarvestFrequencyDistributionHistogramResponse> GetHarvestFrequencyDistributionHistogramAsync(DateTime? startDate = null, DateTime? endDate = null, int binCount = 10, CancellationToken ct = default);
    Task<AvgHarvestSizeDistributionHistogramResponse> GetAvgHarvestSizeDistributionHistogramAsync(DateTime? startDate = null, DateTime? endDate = null, int binCount = 10, CancellationToken ct = default);

    // Area chart endpoints
    Task<CumulativeYieldResponse> GetCumulativeYieldAreaChartAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<CumulativeHarvestCountResponse> GetCumulativeHarvestCountAreaChartAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "daily", CancellationToken ct = default);
    Task<PlantationGrowthResponse> GetPlantationGrowthAreaChartAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);
    Task<TreePopulationGrowthResponse> GetTreePopulationGrowthAreaChartAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", CancellationToken ct = default);
    Task<CumulativeActiveTreesResponse> GetCumulativeActiveTreesAreaChartAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "weekly", CancellationToken ct = default);
    Task<StackedYieldByPlantationResponse> GetStackedYieldByPlantationAreaChartAsync(DateTime? startDate = null, DateTime? endDate = null, string interval = "monthly", int limit = 5, CancellationToken ct = default);
}

