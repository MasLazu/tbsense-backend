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

public record EnvironmentalMetricValue(
    double AirTemperature,
    double SoilTemperature,
    double SoilMoisture
);

public record EnvironmentalAveragesResponse(
    EnvironmentalMetricValue Metrics
);

// Timeseries Response Models

public record EnvironmentalTimeseriesDataPoint(
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

public record EnvironmentalTimeseriesResponse(
    List<EnvironmentalTimeseriesDataPoint> DataPoints
);

public record HarvestTimeseriesDataPoint(
    DateTime Timestamp,
    double TotalYieldKg,
    int HarvestCount,
    double AverageYieldPerHarvest
);

public record HarvestTimeseriesResponse(
    List<HarvestTimeseriesDataPoint> DataPoints
);

public record PlantationGrowthDataPoint(
    DateTime Timestamp,
    int TotalPlantations,
    int ActivePlantations,
    int TotalTrees,
    double TotalLandAreaHectares
);

public record PlantationGrowthTimeseriesResponse(
    List<PlantationGrowthDataPoint> DataPoints
);
