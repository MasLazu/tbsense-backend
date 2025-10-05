namespace TbSense.Backend.Endpoints.Models;

public record PlantationIdRequest
{
    public Guid PlantationId { get; set; }
}

public record PlantationDashboardQueryRequest
{
    public Guid PlantationId { get; set; }
    public string? Period { get; set; }
    public int? Days { get; set; }
}

public record PlantationDateFilterRequest
{
    public Guid PlantationId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public record PlantationTimeseriesRequest
{
    public Guid PlantationId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "hourly", "daily", "weekly", "monthly"
}

public record GetTopTreesByMetricChartRequest
{
    public Guid PlantationId { get; set; }
    public string? Period { get; set; }
    public int? Days { get; set; }
    public string? MetricType { get; set; }
    public int? Limit { get; set; }
}

public record GetWeeklyHarvestPerformanceChartRequest
{
    public Guid PlantationId { get; set; }
    public int? Weeks { get; set; }
}

// Histogram request models
public record PlantationHistogramDateFilterRequest
{
    public Guid PlantationId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? BinCount { get; set; }
}

public record PlantationHistogramDaysRequest
{
    public Guid PlantationId { get; set; }
    public int? Days { get; set; }
    public int? BinCount { get; set; }
}


public record PlantationHistogramBinCountRequest
{
    public Guid PlantationId { get; set; }
    public int? BinCount { get; set; }
}

// Area chart request models
public record PlantationAreaChartRequest
{
    public Guid PlantationId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "daily", "weekly", "monthly"
}
