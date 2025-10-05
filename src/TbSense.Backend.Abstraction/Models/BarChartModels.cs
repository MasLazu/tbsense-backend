namespace TbSense.Backend.Abstraction.Models;

// Global Dashboard Bar Chart Models

public record PlantationYieldComparisonItem(
    Guid PlantationId,
    string PlantationName,
    double TotalYieldKg,
    int HarvestCount
);

public record PlantationYieldComparisonResponse(
    List<PlantationYieldComparisonItem> Items,
    double TotalYield,
    int TotalHarvests
);

public record PlantationAvgHarvestComparisonItem(
    Guid PlantationId,
    string PlantationName,
    double AverageYieldKg,
    int HarvestCount
);

public record PlantationAvgHarvestComparisonResponse(
    List<PlantationAvgHarvestComparisonItem> Items,
    int TotalPlantations
);

public record PlantationTreeCountComparisonItem(
    Guid PlantationId,
    string PlantationName,
    int TreeCount,
    double LandAreaHectares,
    double TreesPerHectare
);

public record PlantationTreeCountComparisonResponse(
    List<PlantationTreeCountComparisonItem> Items,
    int TotalTrees
);

public record PlantationActivityComparisonItem(
    Guid PlantationId,
    string PlantationName,
    int ActiveTrees,
    int InactiveTrees,
    int TotalTrees
);

public record PlantationActivityComparisonResponse(
    List<PlantationActivityComparisonItem> Items,
    int TotalActiveTrees,
    int TotalInactiveTrees
);

public record PlantationHarvestFrequencyItem(
    Guid PlantationId,
    string PlantationName,
    int HarvestCount,
    double TotalYieldKg,
    double AverageYieldKg
);

public record PlantationHarvestFrequencyResponse(
    List<PlantationHarvestFrequencyItem> Items,
    int TotalHarvests
);

// Plantation Dashboard Bar Chart Models

public record TopTreeByMetricItem(
    Guid TreeId,
    double AverageValue,
    double MinValue,
    double MaxValue,
    int ReadingCount
);

public record TopTreeByMetricResponse(
    Guid PlantationId,
    string PlantationName,
    string MetricType,
    List<TopTreeByMetricItem> Items,
    int TotalTrees
);

public record MonthlyHarvestComparisonItem(
    int Year,
    int Month,
    string MonthYear,
    double TotalYieldKg,
    int HarvestCount,
    double AverageYieldKg
);

public record MonthlyHarvestComparisonResponse(
    Guid PlantationId,
    string PlantationName,
    List<MonthlyHarvestComparisonItem> Items,
    double TotalYield
);

public record MonthlyTreeActivityItem(
    int Year,
    int Month,
    string MonthYear,
    int ActiveTrees,
    int InactiveTrees,
    int TotalTrees
);

public record MonthlyTreeActivityResponse(
    Guid PlantationId,
    string PlantationName,
    List<MonthlyTreeActivityItem> Items
);

public record TreesByZoneComparisonItem(
    string Zone,
    int TreeCount,
    double AverageAirTemperature,
    double AverageSoilMoisture
);

public record TreesByZoneComparisonResponse(
    Guid PlantationId,
    string PlantationName,
    List<TreesByZoneComparisonItem> Items,
    int TotalTrees
);

public record WeeklyHarvestPerformanceItem(
    int Year,
    int Week,
    string WeekLabel,
    DateTime WeekStartDate,
    double TotalYieldKg,
    int HarvestCount,
    double AverageYieldKg
);

public record WeeklyHarvestPerformanceResponse(
    Guid PlantationId,
    string PlantationName,
    List<WeeklyHarvestPerformanceItem> Items,
    double TotalYield
);

// Tree Dashboard Bar Chart Models

public record DailyMetricComparisonItem(
    DateTime Date,
    double AverageAirTemperature,
    double AverageSoilTemperature,
    double AverageSoilMoisture,
    int ReadingCount
);

public record DailyMetricComparisonResponse(
    Guid TreeId,
    List<DailyMetricComparisonItem> Items
);

public record HourlyAverageComparisonItem(
    int Hour,
    string HourLabel,
    double AverageAirTemperature,
    double AverageSoilTemperature,
    double AverageSoilMoisture,
    int ReadingCount
);

public record HourlyAverageComparisonResponse(
    Guid TreeId,
    List<HourlyAverageComparisonItem> Items
);

public record WeeklyMetricComparisonItem(
    int Year,
    int Week,
    string WeekLabel,
    DateTime WeekStartDate,
    double AverageAirTemperature,
    double AverageSoilTemperature,
    double AverageSoilMoisture,
    int ReadingCount
);

public record WeeklyMetricComparisonResponse(
    Guid TreeId,
    List<WeeklyMetricComparisonItem> Items
);

public record MetricRangeComparisonItem(
    string MetricType,
    double MinValue,
    double AverageValue,
    double MaxValue,
    int ReadingCount
);

public record MetricRangeComparisonResponse(
    Guid TreeId,
    List<MetricRangeComparisonItem> Items,
    int Days
);
