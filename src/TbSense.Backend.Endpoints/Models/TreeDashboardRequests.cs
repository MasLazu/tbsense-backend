namespace TbSense.Backend.Endpoints.Models;

public record TreeIdRequest
{
    public Guid TreeId { get; set; }
}

public record TreeDashboardQueryRequest
{
    public Guid TreeId { get; set; }
    public int? Days { get; set; }
}

public record TreeTimeseriesRequest
{
    public Guid TreeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "hourly", "daily", "weekly", "monthly"
}

public record TreeMetricRangesRequest
{
    public Guid TreeId { get; set; }
    public string MetricType { get; set; } = "AirTemperature"; // "AirTemperature", "SoilTemperature", "SoilMoisture"
    public int? Days { get; set; }
}

public record GetWeeklyMetricsComparisonChartRequest
{
    public Guid TreeId { get; set; }
    public int? Weeks { get; set; }
}

// Histogram request models
public record TreeHistogramRequest
{
    public Guid TreeId { get; set; }
    public int? Days { get; set; }
    public int? BinCount { get; set; }
}

// Area chart request models
public record TreeAreaChartRequest
{
    public Guid TreeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "daily", "weekly", "monthly"
}

public record TreeThresholdAreaChartRequest
{
    public Guid TreeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "daily", "weekly", "monthly"
    public double? Threshold { get; set; }
}
