namespace TbSense.Backend.Abstraction.Models;

// Histogram bin item
public record HistogramBinItem(
    double RangeStart,
    double RangeEnd,
    string RangeLabel,
    int Count,
    double Percentage
);

// Generic histogram response
public record HistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

// Global Dashboard Histogram Responses
public record YieldDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record PlantationSizeDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record TreeDensityDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record HarvestFrequencyDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record AvgHarvestSizeDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

// Plantation Dashboard Histogram Responses
public record TreeYieldDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record AirTemperatureDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record SoilTemperatureDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record SoilMoistureDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);

public record TreeAgeDistributionHistogramResponse(
    List<HistogramBinItem> Bins,
    int TotalCount,
    double MinValue,
    double MaxValue,
    double Average,
    double Median
);
