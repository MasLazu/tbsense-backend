namespace TbSense.Backend.Abstraction.Models;

// Single data point for area chart
public record AreaChartDataPoint(
    DateTime Date,
    double Value,
    double CumulativeValue
);

// For stacked area charts
public record StackedAreaChartDataPoint(
    DateTime Date,
    Dictionary<string, double> Values,
    double TotalCumulative
);

// Global Dashboard Area Chart Responses
public record CumulativeYieldResponse(
    List<AreaChartDataPoint> DataPoints,
    double TotalYield,
    double AverageDailyIncrease,
    string Interval
);

public record CumulativeHarvestCountResponse(
    List<AreaChartDataPoint> DataPoints,
    double TotalHarvests,
    double AverageDailyIncrease,
    string Interval
);

public record PlantationGrowthResponse(
    List<AreaChartDataPoint> DataPoints,
    double TotalPlantations,
    double AverageMonthlyIncrease,
    string Interval
);

public record TreePopulationGrowthResponse(
    List<AreaChartDataPoint> DataPoints,
    double TotalTrees,
    double AverageMonthlyIncrease,
    string Interval
);

public record CumulativeActiveTreesResponse(
    List<AreaChartDataPoint> DataPoints,
    double TotalActiveTrees,
    double AverageWeeklyIncrease,
    string Interval
);

public record StackedYieldByPlantationResponse(
    List<StackedAreaChartDataPoint> DataPoints,
    Dictionary<string, double> TotalsByPlantation,
    double GrandTotal,
    string Interval,
    int PlantationCount
);

// Plantation Dashboard Area Chart Responses
public record PlantationCumulativeHarvestYieldResponse(
    Guid PlantationId,
    string PlantationName,
    List<AreaChartDataPoint> DataPoints,
    double TotalYield,
    double AverageIncrease,
    string Interval
);

public record PlantationCumulativeHarvestCountResponse(
    Guid PlantationId,
    string PlantationName,
    List<AreaChartDataPoint> DataPoints,
    double TotalHarvests,
    double AverageIncrease,
    string Interval
);

public record TreeMonitoringAdoptionResponse(
    Guid PlantationId,
    string PlantationName,
    List<AreaChartDataPoint> DataPoints,
    double TotalTreesWithMetrics,
    double AverageWeeklyIncrease,
    string Interval
);

public record CumulativeMetricReadingsResponse(
    Guid PlantationId,
    string PlantationName,
    List<AreaChartDataPoint> DataPoints,
    double TotalReadings,
    double AverageDailyIncrease,
    string Interval
);

public record StackedMetricsByTypeResponse(
    Guid PlantationId,
    string PlantationName,
    List<StackedAreaChartDataPoint> DataPoints,
    Dictionary<string, double> TotalsByMetricType,
    double GrandTotal,
    string Interval
);

// Tree Dashboard Area Chart Responses
public record TreeCumulativeMetricReadingsResponse(
    Guid TreeId,
    List<AreaChartDataPoint> DataPoints,
    double TotalReadings,
    double AverageDailyIncrease,
    string Interval
);

public record TreeStackedMetricsByTypeResponse(
    Guid TreeId,
    List<StackedAreaChartDataPoint> DataPoints,
    Dictionary<string, double> TotalsByMetricType,
    double GrandTotal,
    string Interval
);

public record CumulativeTemperatureExposureResponse(
    Guid TreeId,
    List<AreaChartDataPoint> DataPoints,
    double TotalExposureHours,
    double Threshold,
    string MetricType,
    string Interval
);

public record CumulativeMoistureDeficitResponse(
    Guid TreeId,
    List<AreaChartDataPoint> DataPoints,
    double TotalDeficitHours,
    double Threshold,
    string Interval
);
