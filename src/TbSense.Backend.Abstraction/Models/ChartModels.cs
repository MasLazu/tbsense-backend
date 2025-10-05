namespace TbSense.Backend.Abstraction.Models;

// Global Dashboard Pie Chart Models

public record PlantationDistributionItem(
    Guid PlantationId,
    string PlantationName,
    int TreeCount,
    double Percentage
);

public record PlantationDistributionResponse(
    List<PlantationDistributionItem> Items,
    int TotalTrees
);

public record PlantationLandDistributionItem(
    Guid PlantationId,
    string PlantationName,
    double LandAreaHectares,
    double Percentage
);

public record PlantationLandDistributionResponse(
    List<PlantationLandDistributionItem> Items,
    double TotalLandAreaHectares
);

public record HarvestDistributionItem(
    Guid PlantationId,
    string PlantationName,
    double TotalYieldKg,
    int HarvestCount,
    double Percentage
);

public record HarvestDistributionResponse(
    List<HarvestDistributionItem> Items,
    double TotalYieldKg,
    int TotalHarvests
);

public record TreeActivityStatusItem(
    string Status,
    int TreeCount,
    double Percentage
);

public record TreeActivityStatusResponse(
    List<TreeActivityStatusItem> Items,
    int TotalTrees
);

// Plantation Dashboard Pie Chart Models

public record PlantationTreeActivityItem(
    string Status,
    int TreeCount,
    double Percentage
);

public record PlantationTreeActivityResponse(
    Guid PlantationId,
    string PlantationName,
    List<PlantationTreeActivityItem> Items,
    int TotalTrees
);

public record MonthlyHarvestDistributionItem(
    string MonthYear,
    double TotalYieldKg,
    int HarvestCount,
    double Percentage
);

public record MonthlyHarvestDistributionResponse(
    Guid PlantationId,
    string PlantationName,
    List<MonthlyHarvestDistributionItem> Items,
    double TotalYieldKg
);

public record EnvironmentalZoneItem(
    string Zone,
    int TreeCount,
    double Percentage,
    string Description
);

public record EnvironmentalZonesResponse(
    Guid PlantationId,
    string PlantationName,
    List<EnvironmentalZoneItem> Items,
    int TotalTrees
);

// Tree Dashboard Pie Chart Models

public record ReadingDistributionItem(
    string TimeOfDay,
    int ReadingCount,
    double Percentage
);

public record ReadingDistributionResponse(
    Guid TreeId,
    List<ReadingDistributionItem> Items,
    int TotalReadings
);

public record MetricRangeItem(
    string Range,
    int ReadingCount,
    double Percentage
);

public record MetricRangesResponse(
    Guid TreeId,
    string MetricType,
    List<MetricRangeItem> Items,
    int TotalReadings
);
