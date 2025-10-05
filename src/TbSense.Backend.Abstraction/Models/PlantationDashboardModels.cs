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
    double Temperature,
    double SoilMoisture,
    double Humidity,
    double LightIntensity,
    DateTime LastUpdated
);

public record PlantationHealthStatusResponse(
    Dictionary<string, string> Status
);

public record EnvironmentalAverageResponse(
    double Avg,
    double Min,
    double Max
);

public record PlantationEnvironmentalAveragesResponse(
    Dictionary<string, EnvironmentalAverageResponse> Metrics
);
