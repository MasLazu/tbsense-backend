using Microsoft.EntityFrameworkCore;
using TbSense.Backend.Interfaces;
using TbSense.Backend.EfCore.Data;
using TbSense.Backend.Domain.Entities;
using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.EfCore.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly TbSenseBackendDbContext _context;

    public DashboardRepository(TbSenseBackendDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PlantationsSummaryResponse> GetPlantationsSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        // Get all plantations (date filters don't apply to plantation count)
        int total = await _context.Plantations.CountAsync(ct);

        DateTime now = DateTime.UtcNow;

        // If dates are provided, use them for activity check, otherwise use default ranges
        DateTime activityStartForMetrics = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-7);
        DateTime activityStartForHarvests = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);
        DateTime activityEnd = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        int active = await _context.Plantations
            .Where(p => p.Trees.Any(t => t.Metrics.Any(m => m.Timestamp >= activityStartForMetrics && m.Timestamp <= activityEnd)) ||
                       p.Harvests.Any(h => h.HarvestDate >= activityStartForHarvests && h.HarvestDate <= activityEnd))
            .CountAsync(ct);

        return new PlantationsSummaryResponse(total, active);
    }

    public async Task<TreesSummaryResponse> GetTreesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        // Get all plantations with land area (date filters don't apply to tree/plantation counts)
        IQueryable<Plantation> query = _context.Plantations.Where(p => p.LandAreaHectares > 0);

        var data = await query
            .Select(p => new
            {
                TreeCount = p.Trees.Count,
                LandArea = p.LandAreaHectares
            })
            .ToListAsync(ct);

        int total = data.Sum(d => d.TreeCount);
        double totalLandArea = data.Sum(d => (double)d.LandArea);
        double averagePerHectare = totalLandArea > 0 ? total / totalLandArea : 0;

        return new TreesSummaryResponse(total, averagePerHectare);
    }

    public async Task<LandAreaSummaryResponse> GetLandAreaSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        // Get all plantations (date filters don't apply to land area counts)
        IQueryable<Plantation> query = _context.Plantations.AsQueryable();

        double totalHectares = await query
            .SumAsync(p => (double)p.LandAreaHectares, ct);

        double utilized = await query
            .Where(p => p.Trees.Any())
            .SumAsync(p => (double)p.LandAreaHectares, ct);

        double utilizationRate = totalHectares > 0 ? utilized / totalHectares * 100 : 0;

        return new LandAreaSummaryResponse(totalHectares, utilized, utilizationRate);
    }

    public async Task<HarvestSummaryResponse> GetHarvestSummaryAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var harvests = await _context.PlantationHarvests
            .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
            .Select(h => new { h.YieldKg, h.Plantation.LandAreaHectares })
            .ToListAsync(ct);

        double totalYieldKg = harvests.Sum(h => (double)h.YieldKg);
        int harvestCount = harvests.Count;

        double totalLandArea = await _context.Plantations
            .SumAsync(p => (double)p.LandAreaHectares, ct);

        double averageYieldPerHectare = totalLandArea > 0 ? totalYieldKg / totalLandArea : 0;

        return new HarvestSummaryResponse(totalYieldKg, averageYieldPerHectare, harvestCount);
    }

    public async Task<EnvironmentalMetricValue> GetEnvironmentalAveragesAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-1);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<TreeMetric> recentMetrics = await _context.TreeMetrics
            .Where(m => m.Timestamp >= startDate_effective && m.Timestamp <= endDate_effective)
            .ToListAsync(ct);

        if (!recentMetrics.Any())
        {
            return new EnvironmentalMetricValue(0, 0, 0);
        }

        double avgAirTemperature = recentMetrics.Average(m => (double)m.AirTemperature);
        double avgSoilTemperature = recentMetrics.Average(m => (double)m.SoilTemperature);
        double avgSoilMoisture = recentMetrics.Average(m => (double)m.SoilMoisture);

        return new EnvironmentalMetricValue(avgAirTemperature, avgSoilTemperature, avgSoilMoisture);
    }

    public async Task<List<EnvironmentalTimeseriesDataPoint>> GetEnvironmentalTimeseriesAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.Timestamp >= startDate_effective && m.Timestamp <= endDate_effective)
            .OrderBy(m => m.Timestamp)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new List<EnvironmentalTimeseriesDataPoint>();
        }

        List<EnvironmentalTimeseriesDataPoint> dataPoints = interval.ToLower() switch
        {
            "hourly" => metrics
                .GroupBy(m => new DateTime(m.Timestamp.Year, m.Timestamp.Month, m.Timestamp.Day, m.Timestamp.Hour, 0, 0, DateTimeKind.Utc))
                .Select(g => new EnvironmentalTimeseriesDataPoint(
                    g.Key,
                    g.Average(m => (double)m.AirTemperature),
                    g.Min(m => (double)m.AirTemperature),
                    g.Max(m => (double)m.AirTemperature),
                    g.Average(m => (double)m.SoilTemperature),
                    g.Min(m => (double)m.SoilTemperature),
                    g.Max(m => (double)m.SoilTemperature),
                    g.Average(m => (double)m.SoilMoisture),
                    g.Count()
                ))
                .ToList(),

            "weekly" => metrics
                .GroupBy(m => GetWeekStart(m.Timestamp))
                .Select(g => new EnvironmentalTimeseriesDataPoint(
                    g.Key,
                    g.Average(m => (double)m.AirTemperature),
                    g.Min(m => (double)m.AirTemperature),
                    g.Max(m => (double)m.AirTemperature),
                    g.Average(m => (double)m.SoilTemperature),
                    g.Min(m => (double)m.SoilTemperature),
                    g.Max(m => (double)m.SoilTemperature),
                    g.Average(m => (double)m.SoilMoisture),
                    g.Count()
                ))
                .ToList(),

            "monthly" => metrics
                .GroupBy(m => new DateTime(m.Timestamp.Year, m.Timestamp.Month, 1, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new EnvironmentalTimeseriesDataPoint(
                    g.Key,
                    g.Average(m => (double)m.AirTemperature),
                    g.Min(m => (double)m.AirTemperature),
                    g.Max(m => (double)m.AirTemperature),
                    g.Average(m => (double)m.SoilTemperature),
                    g.Min(m => (double)m.SoilTemperature),
                    g.Max(m => (double)m.SoilTemperature),
                    g.Average(m => (double)m.SoilMoisture),
                    g.Count()
                ))
                .ToList(),

            _ => metrics // daily (default)
                .GroupBy(m => new DateTime(m.Timestamp.Year, m.Timestamp.Month, m.Timestamp.Day, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new EnvironmentalTimeseriesDataPoint(
                    g.Key,
                    g.Average(m => (double)m.AirTemperature),
                    g.Min(m => (double)m.AirTemperature),
                    g.Max(m => (double)m.AirTemperature),
                    g.Average(m => (double)m.SoilTemperature),
                    g.Min(m => (double)m.SoilTemperature),
                    g.Max(m => (double)m.SoilTemperature),
                    g.Average(m => (double)m.SoilMoisture),
                    g.Count()
                ))
                .ToList()
        };

        return dataPoints;
    }

    public async Task<List<HarvestTimeseriesDataPoint>> GetHarvestTimeseriesAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "monthly",
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<PlantationHarvest> harvests = await _context.PlantationHarvests
            .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
            .OrderBy(h => h.HarvestDate)
            .ToListAsync(ct);

        if (!harvests.Any())
        {
            return new List<HarvestTimeseriesDataPoint>();
        }

        // Group by interval and calculate totals
        List<HarvestTimeseriesDataPoint> dataPoints = interval.ToLower() switch
        {
            "daily" => harvests
                .GroupBy(h => new DateTime(h.HarvestDate.Year, h.HarvestDate.Month, h.HarvestDate.Day, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new HarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList(),

            "weekly" => harvests
                .GroupBy(h => GetWeekStart(h.HarvestDate))
                .Select(g => new HarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList(),

            "yearly" => harvests
                .GroupBy(h => new DateTime(h.HarvestDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new HarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList(),

            _ => harvests // monthly (default)
                .GroupBy(h => new DateTime(h.HarvestDate.Year, h.HarvestDate.Month, 1, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new HarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList()
        };

        return dataPoints;
    }

    public async Task<List<PlantationGrowthDataPoint>> GetPlantationGrowthTimeseriesAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "monthly",
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<Plantation> plantations = await _context.Plantations
            .Include(p => p.Trees)
            .Where(p => p.CreatedAt <= endDate_effective)
            .ToListAsync(ct);

        if (!plantations.Any())
        {
            return new List<PlantationGrowthDataPoint>();
        }

        List<DateTime> timePoints = GenerateTimePoints(startDate_effective, endDate_effective, interval);

        var dataPoints = timePoints.Select(timestamp =>
        {
            var existingPlantations = plantations
                .Where(p => p.CreatedAt <= timestamp)
                .ToList();

            int totalPlantations = existingPlantations.Count;
            double totalLandArea = existingPlantations.Sum(p => p.LandAreaHectares);

            int totalTrees = existingPlantations
                .SelectMany(p => p.Trees)
                .Count(t => t.CreatedAt <= timestamp);

            DateTime sevenDaysBeforeTimestamp = timestamp.AddDays(-7);
            DateTime thirtyDaysBeforeTimestamp = timestamp.AddDays(-30);

            int activePlantations = existingPlantations
                .Count(p =>
                    p.Trees.Any(t => t.Metrics.Any(m => m.Timestamp >= sevenDaysBeforeTimestamp && m.Timestamp <= timestamp)) ||
                    p.Harvests.Any(h => h.HarvestDate >= thirtyDaysBeforeTimestamp && h.HarvestDate <= timestamp));

            return new PlantationGrowthDataPoint(
                timestamp,
                totalPlantations,
                activePlantations,
                totalTrees,
                totalLandArea
            );
        }).ToList();

        return dataPoints;
    }

    private static List<DateTime> GenerateTimePoints(DateTime startDate, DateTime endDate, string interval)
    {
        List<DateTime> timePoints = new();
        DateTime current = interval.ToLower() switch
        {
            "daily" => new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc),
            "weekly" => GetWeekStart(startDate),
            "yearly" => new DateTime(startDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            _ => new DateTime(startDate.Year, startDate.Month, 1, 0, 0, 0, DateTimeKind.Utc) // monthly default
        };

        while (current <= endDate)
        {
            timePoints.Add(current);
            current = interval.ToLower() switch
            {
                "daily" => current.AddDays(1),
                "weekly" => current.AddDays(7),
                "yearly" => current.AddYears(1),
                _ => current.AddMonths(1) // monthly default
            };
        }

        return timePoints;
    }

    private static DateTime GetWeekStart(DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(-diff);
    }

    // Chart Methods

    public async Task<List<PlantationDistributionItem>> GetPlantationsByTreesChartAsync(CancellationToken ct = default)
    {
        var data = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                TreeCount = p.Trees.Count
            })
            .ToListAsync(ct);

        int totalTrees = data.Sum(d => d.TreeCount);

        return data
            .OrderByDescending(d => d.TreeCount)
            .Select(d => new PlantationDistributionItem(
                d.Id,
                d.Name,
                d.TreeCount,
                totalTrees > 0 ? Math.Round((double)d.TreeCount / totalTrees * 100, 2) : 0
            ))
            .ToList();
    }

    public async Task<List<PlantationLandDistributionItem>> GetPlantationsByLandAreaChartAsync(CancellationToken ct = default)
    {
        var data = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.LandAreaHectares
            })
            .ToListAsync(ct);

        double totalLandArea = data.Sum(d => d.LandAreaHectares);

        return data
            .OrderByDescending(d => d.LandAreaHectares)
            .Select(d => new PlantationLandDistributionItem(
                d.Id,
                d.Name,
                d.LandAreaHectares,
                totalLandArea > 0 ? Math.Round(d.LandAreaHectares / totalLandArea * 100, 2) : 0
            ))
            .ToList();
    }

    public async Task<List<HarvestDistributionItem>> GetHarvestByPlantationChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        // Default to last 12 months
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var data = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                Harvests = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Select(h => h.YieldKg)
                    .ToList()
            })
            .ToListAsync(ct);

        var summary = data
            .Select(d => new
            {
                d.Id,
                d.Name,
                TotalYieldKg = d.Harvests.Sum(y => (double)y),
                HarvestCount = d.Harvests.Count
            })
            .Where(d => d.HarvestCount > 0)
            .ToList();

        double totalYieldKg = summary.Sum(d => d.TotalYieldKg);

        return summary
            .OrderByDescending(d => d.TotalYieldKg)
            .Select(d => new HarvestDistributionItem(
                d.Id,
                d.Name,
                Math.Round(d.TotalYieldKg, 2),
                d.HarvestCount,
                totalYieldKg > 0 ? Math.Round(d.TotalYieldKg / totalYieldKg * 100, 2) : 0
            ))
            .ToList();
    }

    public async Task<List<TreeActivityStatusItem>> GetTreeActivityStatusChartAsync(CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime sevenDaysAgo = now.AddDays(-7);

        List<Tree> allTrees = await _context.Trees.ToListAsync(ct);
        List<Guid> treeIdsWithRecentMetrics = await _context.TreeMetrics
            .Where(m => m.Timestamp >= sevenDaysAgo)
            .Select(m => m.TreeId)
            .Distinct()
            .ToListAsync(ct);

        int activeTrees = treeIdsWithRecentMetrics.Count;
        int inactiveTrees = allTrees.Count - activeTrees;
        int totalTrees = allTrees.Count;

        return new List<TreeActivityStatusItem>
        {
            new TreeActivityStatusItem(
                "Active",
                activeTrees,
                totalTrees > 0 ? Math.Round((double)activeTrees / totalTrees * 100, 2) : 0
            ),
            new TreeActivityStatusItem(
                "Inactive",
                inactiveTrees,
                totalTrees > 0 ? Math.Round((double)inactiveTrees / totalTrees * 100, 2) : 0
            )
        };
    }

    public async Task<List<PlantationYieldComparisonItem>> GetTopPlantationsByYieldChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 10,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<PlantationYieldComparisonItem> items = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                TotalYieldKg = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Sum(h => (double)h.YieldKg),
                HarvestCount = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Count()
            })
            .Where(p => p.HarvestCount > 0)
            .OrderByDescending(p => p.TotalYieldKg)
            .Take(limit)
            .Select(p => new PlantationYieldComparisonItem(
                p.Id,
                p.Name,
                p.TotalYieldKg,
                p.HarvestCount
            ))
            .ToListAsync(ct);

        return items;
    }

    public async Task<List<PlantationAvgHarvestComparisonItem>> GetTopPlantationsByAvgHarvestChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 10,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var items = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                TotalYield = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Sum(h => (double)h.YieldKg),
                HarvestCount = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Count()
            })
            .Where(p => p.HarvestCount > 0)
            .OrderByDescending(p => p.TotalYield / p.HarvestCount)
            .Take(limit)
            .ToListAsync(ct);

        return items.Select(p => new PlantationAvgHarvestComparisonItem(
            p.Id,
            p.Name,
            Math.Round(p.TotalYield / p.HarvestCount, 2),
            p.HarvestCount
        )).ToList();
    }

    public async Task<List<PlantationTreeCountComparisonItem>> GetTreeCountByPlantationChartAsync(
        int limit = 10,
        CancellationToken ct = default)
    {
        List<PlantationTreeCountComparisonItem> items = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                TreeCount = p.Trees.Count,
                p.LandAreaHectares,
                TreesPerHectare = p.LandAreaHectares > 0 ? Math.Round(p.Trees.Count / p.LandAreaHectares, 2) : 0
            })
            .OrderByDescending(p => p.TreeCount)
            .Take(limit)
            .Select(p => new PlantationTreeCountComparisonItem(
                p.Id,
                p.Name,
                p.TreeCount,
                p.LandAreaHectares,
                p.TreesPerHectare
            ))
            .ToListAsync(ct);

        return items;
    }

    public async Task<List<PlantationActivityComparisonItem>> GetTreeActivityByPlantationChartAsync(
        CancellationToken ct = default)
    {
        DateTime sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

        List<PlantationActivityComparisonItem> items = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                ActiveTrees = p.Trees.Count(t => t.Metrics.Any(m => m.Timestamp >= sevenDaysAgo)),
                InactiveTrees = p.Trees.Count(t => !t.Metrics.Any(m => m.Timestamp >= sevenDaysAgo)),
                TotalTrees = p.Trees.Count
            })
            .OrderByDescending(p => p.TotalTrees)
            .Select(p => new PlantationActivityComparisonItem(
                p.Id,
                p.Name,
                p.ActiveTrees,
                p.InactiveTrees,
                p.TotalTrees
            ))
            .ToListAsync(ct);

        return items;
    }

    public async Task<List<PlantationHarvestFrequencyItem>> GetHarvestFrequencyByPlantationChartAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var items = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                HarvestCount = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Count(),
                TotalYield = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Sum(h => (double)h.YieldKg)
            })
            .Where(p => p.HarvestCount > 0)
            .OrderByDescending(p => p.HarvestCount)
            .ToListAsync(ct);

        return items.Select(p => new PlantationHarvestFrequencyItem(
            p.Id,
            p.Name,
            p.HarvestCount,
            p.TotalYield,
            Math.Round(p.TotalYield / p.HarvestCount, 2)
        )).ToList();
    }

    // Histogram methods
    public async Task<List<double>> GetYieldDistributionDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<double> yields = await _context.Plantations
            .Select(p => p.Harvests
                .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                .Sum(h => (double)h.YieldKg))
            .Where(y => y > 0)
            .ToListAsync(ct);

        return yields;
    }

    public async Task<List<double>> GetPlantationSizeDistributionDataAsync(CancellationToken ct = default)
    {
        List<double> sizes = await _context.Plantations
            .Select(p => (double)p.LandAreaHectares)
            .Where(s => s > 0)
            .ToListAsync(ct);

        return sizes;
    }

    public async Task<List<double>> GetTreeDensityDistributionDataAsync(CancellationToken ct = default)
    {
        List<double> densities = await _context.Plantations
            .Where(p => p.LandAreaHectares > 0)
            .Select(p => (double)p.Trees.Count / (double)p.LandAreaHectares)
            .ToListAsync(ct);

        return densities;
    }

    public async Task<List<double>> GetHarvestFrequencyDistributionDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<double> frequencies = await _context.Plantations
            .Select(p => (double)p.Harvests
                .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                .Count())
            .Where(f => f > 0)
            .ToListAsync(ct);

        return frequencies;
    }

    public async Task<List<double>> GetAvgHarvestSizeDistributionDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-12);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var data = await _context.Plantations
            .Select(p => new
            {
                TotalYield = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Sum(h => (double)h.YieldKg),
                HarvestCount = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Count()
            })
            .Where(p => p.HarvestCount > 0)
            .ToListAsync(ct);

        return data.Select(p => p.TotalYield / p.HarvestCount).ToList();
    }

    // Area chart data methods
    public async Task<List<(DateTime Date, double Value)>> GetCumulativeYieldDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddYears(-1);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var harvests = await _context.Plantations
            .SelectMany(p => p.Harvests)
            .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
            .OrderBy(h => h.HarvestDate)
            .Select(h => new { h.HarvestDate, Yield = (double)h.YieldKg })
            .ToListAsync(ct);

        return harvests.Select(h => (h.HarvestDate, h.Yield)).ToList();
    }

    public async Task<List<(DateTime Date, int Count)>> GetCumulativeHarvestCountDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddYears(-1);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<DateTime> harvests = await _context.Plantations
            .SelectMany(p => p.Harvests)
            .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
            .OrderBy(h => h.HarvestDate)
            .Select(h => h.HarvestDate)
            .ToListAsync(ct);

        return harvests.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, int Count)>> GetPlantationGrowthDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddYears(-2);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<DateTime> plantations = await _context.Plantations
            .Where(p => p.CreatedAt >= startDate_effective && p.CreatedAt <= endDate_effective)
            .OrderBy(p => p.CreatedAt)
            .Select(p => p.CreatedAt.DateTime)
            .ToListAsync(ct);

        return plantations.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, int Count)>> GetTreePopulationGrowthDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddYears(-2);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<DateTime> trees = await _context.Trees
            .Where(t => t.CreatedAt >= startDate_effective && t.CreatedAt <= endDate_effective)
            .OrderBy(t => t.CreatedAt)
            .Select(t => t.CreatedAt.DateTime)
            .ToListAsync(ct);

        return trees.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, int Count)>> GetCumulativeActiveTreesDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddYears(-1);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<DateTime> treeMetrics = await _context.TreeMetrics
            .Where(m => m.Timestamp >= startDate_effective && m.Timestamp <= endDate_effective)
            .GroupBy(m => new { m.Timestamp.Date, m.TreeId })
            .Select(g => new { g.Key.Date, g.Key.TreeId })
            .OrderBy(x => x.Date)
            .Select(x => x.Date)
            .ToListAsync(ct);

        return treeMetrics.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, string PlantationName, double Yield)>> GetStackedYieldByPlantationDataAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 5,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddYears(-1);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        // Get top plantations by total yield
        List<Guid> topPlantations = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                p.Name,
                TotalYield = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Sum(h => (double)h.YieldKg)
            })
            .OrderByDescending(p => p.TotalYield)
            .Take(limit)
            .Select(p => p.Id)
            .ToListAsync(ct);

        var harvestData = await _context.Plantations
            .Where(p => topPlantations.Contains(p.Id))
            .SelectMany(p => p.Harvests.Select(h => new { h.HarvestDate, p.Name, Yield = (double)h.YieldKg }))
            .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
            .OrderBy(h => h.HarvestDate)
            .ToListAsync(ct);

        return harvestData.Select(h => (h.HarvestDate, h.Name, h.Yield)).ToList();
    }
}


