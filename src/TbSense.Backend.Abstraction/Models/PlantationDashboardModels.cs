namespace TbSense.Backend.Abstraction.Models;

// Plantation-Specific Dashboard Response Models

public record PlantationBasicSummaryResponse(
    Guid Id,
    string Name,
    double LandAreaHectares,
    DateTime PlantedDate,
    int AgeInDays,
    double AgeInYears
);

public record PlantationTreesSummaryResponse(
    int TotalTrees,
    double TreesPerHectare,
    int ActiveTreesWithMetrics
);

public record PlantationHarvestSummaryResponse(
    double TotalYieldKg,
    double YieldPerHectare,
    int HarvestCount,
    double? AveragePerHarvest = null
);

public record PlantationHarvestAllTimeResponse(
    double TotalYieldKg,
    double AverageYieldPerHectare,
    int TotalHarvests,
    DateTime? FirstHarvestDate
);

public record PlantationRankingResponse(
    int Rank,
    int TotalPlantations,
    double Percentile,
    string Category
);

public record PlantationCurrentMetricsResponse(
    double AirTemperature,
    double SoilTemperature,
    double SoilMoisture,
    DateTime LastUpdated
);

public record EnvironmentalAverageValue(
    double Avg,
    double Min,
    double Max
);

public record PlantationEnvironmentalMetrics(
    EnvironmentalAverageValue AirTemperature,
    EnvironmentalAverageValue SoilTemperature,
    EnvironmentalAverageValue SoilMoisture
);

public record PlantationEnvironmentalAveragesResponse(
    PlantationEnvironmentalMetrics Metrics
);

// Plantation-Specific Timeseries Response Models

public record PlantationEnvironmentalTimeseriesDataPoint(
    DateTime Timestamp,
    double AirTemperature,
    double MinAirTemperature,
    double MaxAirTemperature,
    double SoilTemperature,
    double MinSoilTemperature,
    double MaxSoilTemperature,
    double SoilMoisture,
    int SampleCount
);

public record PlantationEnvironmentalTimeseriesResponse(
    List<PlantationEnvironmentalTimeseriesDataPoint> DataPoints
);

public record PlantationHarvestTimeseriesDataPoint(
    DateTime Timestamp,
    double TotalYieldKg,
    int HarvestCount,
    double AverageYieldPerHarvest
);

public record PlantationHarvestTimeseriesResponse(
    List<PlantationHarvestTimeseriesDataPoint> DataPoints
);

public record PlantationTreeGrowthDataPoint(
    DateTime Timestamp,
    int TotalTrees,
    int ActiveTrees,
    double TreesPerHectare
);

public record PlantationTreeGrowthTimeseriesResponse(
    List<PlantationTreeGrowthDataPoint> DataPoints
);
