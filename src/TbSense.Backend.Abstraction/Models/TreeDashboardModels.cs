namespace TbSense.Backend.Abstraction.Models;

// Tree-Specific Dashboard Response Models

public record TreeBasicInfoResponse(
    Guid Id,
    Guid PlantationId,
    string PlantationName,
    double Latitude,
    double Longitude,
    DateTime CreatedAt,
    int AgeInDays,
    bool IsActive
);

public record TreeCurrentMetricsResponse(
    double AirTemperature,
    double SoilTemperature,
    double SoilMoisture,
    DateTime LastUpdated,
    int MinutesSinceLastUpdate
);

public record TreeEnvironmentalAveragesResponse(
    EnvironmentalAverageValue AirTemperature,
    EnvironmentalAverageValue SoilTemperature,
    EnvironmentalAverageValue SoilMoisture,
    int TotalReadings
);

public record TreeEnvironmentalTimeseriesDataPoint(
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

public record TreeEnvironmentalTimeseriesResponse(
    List<TreeEnvironmentalTimeseriesDataPoint> DataPoints
);
