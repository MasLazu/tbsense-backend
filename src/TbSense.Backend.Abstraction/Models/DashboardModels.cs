namespace TbSense.Backend.Abstraction.Models;

// Global Dashboard Response Models

public record PlantationsSummaryResponse(
    int Total,
    int Active
);

public record TreesSummaryResponse(
    int Total,
    double AveragePerHectare
);

public record LandAreaSummaryResponse(
    double TotalHectares,
    double Utilized,
    double UtilizationRate
);

public record HarvestSummaryResponse(
    double TotalYieldKg,
    double AverageYieldPerHectare,
    int HarvestCount
);

public record EnvironmentalMetricResponse(
    double Current,
    double[] Optimal,
    string Status
);

public record EnvironmentalAveragesResponse(
    Dictionary<string, EnvironmentalMetricResponse> Metrics
);

public record EnvironmentalStatusResponse(
    int PlantationsInOptimalRange,
    int PlantationsWithWarnings,
    int PlantationsCritical,
    int Total
);
