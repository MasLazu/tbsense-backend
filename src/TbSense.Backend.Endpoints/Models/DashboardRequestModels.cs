namespace TbSense.Backend.Endpoints.Models;

public record DashboardDateFilterRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public record DashboardTimeseriesRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "hourly", "daily", "weekly", "monthly"
}

public record GetTopPlantationsByYieldChartRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Limit { get; set; }
}

public record GetTopPlantationsByAvgHarvestChartRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Limit { get; set; }
}

public record GetTreeCountByPlantationChartRequest
{
    public int? Limit { get; set; }
}

// Histogram request models
public record HistogramDateFilterRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? BinCount { get; set; }
}

public record HistogramBinCountRequest
{
    public int? BinCount { get; set; }
}

// Area chart request models
public record AreaChartIntervalRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "daily", "weekly", "monthly"
}

public record StackedAreaChartRequest
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Interval { get; set; } // "daily", "weekly", "monthly"
    public int? Limit { get; set; }
}
