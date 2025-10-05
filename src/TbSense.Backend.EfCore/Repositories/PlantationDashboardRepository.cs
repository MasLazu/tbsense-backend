using Microsoft.EntityFrameworkCore;
using TbSense.Backend.Interfaces;
using TbSense.Backend.EfCore.Data;
using TbSense.Backend.Domain.Entities;
using TbSense.Backend.Abstraction.Models;
using System.Globalization;

namespace TbSense.Backend.EfCore.Repositories;

public class PlantationDashboardRepository : IPlantationDashboardRepository
{
    private readonly TbSenseBackendDbContext _context;

    public PlantationDashboardRepository(TbSenseBackendDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PlantationBasicSummaryResponse?> GetPlantationBasicSummaryAsync(Guid plantationId, CancellationToken ct = default)
    {
        var plantation = await _context.Plantations
            .Where(p => p.Id == plantationId)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.LandAreaHectares,
                p.PlantedDate
            })
            .FirstOrDefaultAsync(ct);

        if (plantation == null)
        {
            return null;
        }

        DateTime now = DateTime.UtcNow;
        int ageInDays = (int)(now - plantation.PlantedDate).TotalDays;
        double ageInYears = Math.Round(ageInDays / 365.25, 1);

        return new PlantationBasicSummaryResponse(
            plantation.Id,
            plantation.Name,
            plantation.LandAreaHectares,
            plantation.PlantedDate,
            ageInDays,
            ageInYears
        );
    }

    public async Task<PlantationTreesSummaryResponse> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime sevenDaysAgo = now.AddDays(-7);

        var data = await _context.Plantations
            .Where(p => p.Id == plantationId)
            .Select(p => new
            {
                TotalTrees = p.Trees.Count,
                LandArea = p.LandAreaHectares,
                ActiveTrees = p.Trees.Count(t => t.Metrics.Any(m => m.CreatedAt >= sevenDaysAgo))
            })
            .FirstOrDefaultAsync(ct);

        if (data == null)
        {
            return new PlantationTreesSummaryResponse(0, 0, 0);
        }

        double treesPerHectare = data.LandArea > 0 ? data.TotalTrees / (double)data.LandArea : 0;

        return new PlantationTreesSummaryResponse(data.TotalTrees, treesPerHectare, data.ActiveTrees);
    }

    public async Task<PlantationHarvestSummaryResponse> GetPlantationHarvestSummaryAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        // Default to current month if no dates provided
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var data = await _context.Plantations
            .Where(p => p.Id == plantationId)
            .Select(p => new
            {
                LandArea = p.LandAreaHectares,
                Harvests = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Select(h => h.YieldKg)
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (data == null)
        {
            return new PlantationHarvestSummaryResponse(0, 0, 0, 0);
        }

        double totalYieldKg = data.Harvests.Sum(y => (double)y);
        int harvestCount = data.Harvests.Count;
        double yieldPerHectare = data.LandArea > 0 ? totalYieldKg / (double)data.LandArea : 0;
        double averagePerHarvest = harvestCount > 0 ? totalYieldKg / harvestCount : 0;

        return new PlantationHarvestSummaryResponse(totalYieldKg, yieldPerHectare, harvestCount, averagePerHarvest);
    }

    public async Task<PlantationRankingResponse?> GetPlantationRankingAsync(Guid plantationId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        // Default to current month if no dates provided
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        // Get all plantations with their yield per hectare for the period
        var plantationYields = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                LandArea = p.LandAreaHectares,
                TotalYield = p.Harvests
                    .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
                    .Sum(h => (double)h.YieldKg)
            })
            .Where(p => p.LandArea > 0)
            .ToListAsync(ct);

        if (!plantationYields.Any())
        {
            return null;
        }

        var plantationYieldsWithRank = plantationYields
            .Select(p => new
            {
                p.Id,
                YieldPerHectare = p.TotalYield / (double)p.LandArea
            })
            .OrderByDescending(p => p.YieldPerHectare)
            .Select((p, index) => new
            {
                p.Id,
                p.YieldPerHectare,
                Rank = index + 1
            })
            .ToList();

        var targetPlantation = plantationYieldsWithRank.FirstOrDefault(p => p.Id == plantationId);
        if (targetPlantation == null)
        {
            return null;
        }

        int totalPlantations = plantationYieldsWithRank.Count;
        double percentile = Math.Round((1 - (double)targetPlantation.Rank / totalPlantations) * 100, 1);

        string category = percentile >= 90
            ? "top_performer"
            : percentile >= 75
                ? "high_performer"
                : percentile >= 50 ? "average_performer" : percentile >= 25 ? "below_average" : "needs_improvement";


        return new PlantationRankingResponse(targetPlantation.Rank, totalPlantations, percentile, category);
    }

    public async Task<PlantationCurrentMetricsResponse?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime oneDayAgo = now.AddDays(-1);

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId && m.CreatedAt >= oneDayAgo)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return null;
        }

        double avgAirTemperature = metrics.Average(m => (double)m.AirTemperature);
        double avgSoilTemperature = metrics.Average(m => (double)m.SoilTemperature);
        double avgSoilMoisture = metrics.Average(m => (double)m.SoilMoisture);
        DateTimeOffset lastUpdated = metrics.Max(m => m.CreatedAt);

        return new PlantationCurrentMetricsResponse(avgAirTemperature, avgSoilTemperature, avgSoilMoisture, lastUpdated.DateTime);
    }

    public async Task<PlantationEnvironmentalMetrics> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate = now.AddDays(-days);

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId && m.CreatedAt >= startDate)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new PlantationEnvironmentalMetrics(
                new EnvironmentalAverageValue(0, 0, 0),
                new EnvironmentalAverageValue(0, 0, 0),
                new EnvironmentalAverageValue(0, 0, 0)
            );
        }

        var airTemperatures = metrics.Select(m => (double)m.AirTemperature).ToList();
        var soilTemperatures = metrics.Select(m => (double)m.SoilTemperature).ToList();
        var soilMoistures = metrics.Select(m => (double)m.SoilMoisture).ToList();

        return new PlantationEnvironmentalMetrics(
            new EnvironmentalAverageValue(airTemperatures.Average(), airTemperatures.Min(), airTemperatures.Max()),
            new EnvironmentalAverageValue(soilTemperatures.Average(), soilTemperatures.Min(), soilTemperatures.Max()),
            new EnvironmentalAverageValue(soilMoistures.Average(), soilMoistures.Min(), soilMoistures.Max())
        );
    }

    public async Task<List<PlantationEnvironmentalTimeseriesDataPoint>> GetPlantationEnvironmentalTimeseriesAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string interval = "daily",
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;

        // Default to 30 days ago if no start date provided
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);

        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId &&
                       m.CreatedAt >= startDate_effective &&
                       m.CreatedAt <= endDate_effective)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new List<PlantationEnvironmentalTimeseriesDataPoint>();
        }

        // Group by interval and calculate averages
        List<PlantationEnvironmentalTimeseriesDataPoint> dataPoints = interval.ToLower() switch
        {
            "hourly" => metrics
                .GroupBy(m => new DateTime(m.CreatedAt.Year, m.CreatedAt.Month, m.CreatedAt.Day, m.CreatedAt.Hour, 0, 0, DateTimeKind.Utc))
                .Select(g => new PlantationEnvironmentalTimeseriesDataPoint(
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
                .GroupBy(m => GetWeekStart(m.CreatedAt.UtcDateTime))
                .Select(g => new PlantationEnvironmentalTimeseriesDataPoint(
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
                .GroupBy(m => new DateTime(m.CreatedAt.Year, m.CreatedAt.Month, 1, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new PlantationEnvironmentalTimeseriesDataPoint(
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
                .GroupBy(m => new DateTime(m.CreatedAt.Year, m.CreatedAt.Month, m.CreatedAt.Day, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new PlantationEnvironmentalTimeseriesDataPoint(
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

    public async Task<List<PlantationHarvestTimeseriesDataPoint>> GetPlantationHarvestTimeseriesAsync(
        Guid plantationId,
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
            .Where(h => h.PlantationId == plantationId &&
                       h.HarvestDate >= startDate_effective &&
                       h.HarvestDate <= endDate_effective)
            .OrderBy(h => h.HarvestDate)
            .ToListAsync(ct);

        if (!harvests.Any())
        {
            return new List<PlantationHarvestTimeseriesDataPoint>();
        }

        List<PlantationHarvestTimeseriesDataPoint> dataPoints = interval.ToLower() switch
        {
            "daily" => harvests
                .GroupBy(h => new DateTime(h.HarvestDate.Year, h.HarvestDate.Month, h.HarvestDate.Day, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new PlantationHarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList(),

            "weekly" => harvests
                .GroupBy(h => GetWeekStart(h.HarvestDate))
                .Select(g => new PlantationHarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList(),

            "yearly" => harvests
                .GroupBy(h => new DateTime(h.HarvestDate.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new PlantationHarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList(),

            _ => harvests // monthly (default)
                .GroupBy(h => new DateTime(h.HarvestDate.Year, h.HarvestDate.Month, 1, 0, 0, 0, DateTimeKind.Utc))
                .Select(g => new PlantationHarvestTimeseriesDataPoint(
                    g.Key,
                    g.Sum(h => (double)h.YieldKg),
                    g.Count(),
                    g.Average(h => (double)h.YieldKg)
                ))
                .ToList()
        };

        return dataPoints;
    }

    public async Task<List<PlantationTreeGrowthDataPoint>> GetPlantationTreeGrowthTimeseriesAsync(
        Guid plantationId,
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

        var plantation = await _context.Plantations
            .Where(p => p.Id == plantationId)
            .Select(p => new { p.LandAreaHectares })
            .FirstOrDefaultAsync(ct);

        if (plantation == null)
        {
            return new List<PlantationTreeGrowthDataPoint>();
        }

        List<Tree> trees = await _context.Trees
            .Include(t => t.Metrics)
            .Where(t => t.PlantationId == plantationId && t.CreatedAt <= endDate_effective)
            .ToListAsync(ct);

        if (!trees.Any())
        {
            return new List<PlantationTreeGrowthDataPoint>();
        }

        List<DateTime> timePoints = GenerateTimePoints(startDate_effective, endDate_effective, interval);

        var dataPoints = timePoints.Select(timestamp =>
        {
            var existingTrees = trees.Where(t => t.CreatedAt <= timestamp).ToList();
            int totalTrees = existingTrees.Count;

            DateTime sevenDaysBeforeTimestamp = timestamp.AddDays(-7);
            int activeTrees = existingTrees
                .Count(t => t.Metrics.Any(m => m.CreatedAt >= sevenDaysBeforeTimestamp && m.CreatedAt <= timestamp));

            double treesPerHectare = plantation.LandAreaHectares > 0
                ? totalTrees / plantation.LandAreaHectares
                : 0;

            return new PlantationTreeGrowthDataPoint(
                timestamp,
                totalTrees,
                activeTrees,
                Math.Round(treesPerHectare, 2)
            );
        }).ToList();

        return dataPoints;
    }

    public async Task<List<PlantationTreeActivityItem>> GetPlantationTreeActivityChartAsync(Guid plantationId, CancellationToken ct = default)
    {
        DateTime sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

        int activeTrees = await _context.Trees
            .Where(t => t.PlantationId == plantationId && t.Metrics.Any(m => m.CreatedAt >= sevenDaysAgo))
            .CountAsync(ct);

        int totalTrees = await _context.Trees
            .Where(t => t.PlantationId == plantationId)
            .CountAsync(ct);

        int inactiveTrees = totalTrees - activeTrees;

        var items = new List<PlantationTreeActivityItem>();

        if (activeTrees > 0)
        {
            double activePercentage = totalTrees > 0 ? Math.Round((double)activeTrees / totalTrees * 100, 2) : 0;
            items.Add(new PlantationTreeActivityItem("Active", activeTrees, activePercentage));
        }

        if (inactiveTrees > 0)
        {
            double inactivePercentage = totalTrees > 0 ? Math.Round((double)inactiveTrees / totalTrees * 100, 2) : 0;
            items.Add(new PlantationTreeActivityItem("Inactive", inactiveTrees, inactivePercentage));
        }

        return items;
    }

    public async Task<List<MonthlyHarvestDistributionItem>> GetPlantationHarvestByMonthChartAsync(
        Guid plantationId,
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

        var monthlyData = await _context.PlantationHarvests
            .Where(h => h.PlantationId == plantationId
                && h.HarvestDate >= startDate_effective
                && h.HarvestDate <= endDate_effective)
            .GroupBy(h => new { h.HarvestDate.Year, h.HarvestDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalYieldKg = g.Sum(h => h.YieldKg),
                HarvestCount = g.Count()
            })
            .OrderBy(g => g.Year)
            .ThenBy(g => g.Month)
            .ToListAsync(ct);

        double totalYieldKg = monthlyData.Sum(d => (double)d.TotalYieldKg);

        var items = monthlyData
            .Select(d =>
            {
                string monthName = new DateTime(d.Year, d.Month, 1, 0, 0, 0, DateTimeKind.Utc).ToString("MMM yyyy");
                double percentage = totalYieldKg > 0 ? Math.Round(d.TotalYieldKg / totalYieldKg * 100, 2) : 0;
                return new MonthlyHarvestDistributionItem(monthName, d.TotalYieldKg, d.HarvestCount, percentage);
            })
            .ToList();

        return items;
    }

    public async Task<List<EnvironmentalZoneItem>> GetPlantationEnvironmentalZonesChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var treeEnvironmentalData = await _context.Trees
            .Where(t => t.PlantationId == plantationId)
            .Select(t => new
            {
                t.Id,
                AvgAirTemp = t.Metrics
                    .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
                    .Average(m => (double?)m.AirTemperature),
                AvgSoilMoisture = t.Metrics
                    .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
                    .Average(m => (double?)m.SoilMoisture)
            })
            .Where(t => t.AvgAirTemp.HasValue && t.AvgSoilMoisture.HasValue)
            .ToListAsync(ct);

        if (!treeEnvironmentalData.Any())
        {
            return new List<EnvironmentalZoneItem>();
        }

        // Categorize trees into zones based on air temperature and soil moisture
        var zones = treeEnvironmentalData
            .GroupBy(t =>
            {
                double airTemp = t.AvgAirTemp!.Value;
                double soilMoisture = t.AvgSoilMoisture!.Value;

                if (airTemp < 20 && soilMoisture < 40)
                {
                    return "Cool & Dry Soil";
                }

                else if (airTemp < 20 && soilMoisture >= 40)
                {
                    return "Cool & Moist Soil";
                }

                else if (airTemp >= 20 && airTemp < 30 && soilMoisture < 40)
                {

                    return "Moderate & Dry Soil";
                }

                else if (airTemp >= 20 && airTemp < 30 && soilMoisture >= 40)
                {

                    return "Moderate & Moist Soil";
                }

                else if (airTemp >= 30 && soilMoisture < 40)
                {

                    return "Hot & Dry Soil";
                }
                else
                {

                    return "Hot & Moist Soil";
                }

            })
            .Select(g => new
            {
                ZoneName = g.Key,
                TreeCount = g.Count(),
                AvgAirTemp = Math.Round(g.Average(t => t.AvgAirTemp!.Value), 2),
                AvgSoilMoisture = Math.Round(g.Average(t => t.AvgSoilMoisture!.Value), 2)
            })
            .OrderByDescending(z => z.TreeCount)
            .ToList();

        int totalTrees = zones.Sum(z => z.TreeCount);

        var items = zones
            .Select(z =>
            {
                double percentage = totalTrees > 0 ? Math.Round((double)z.TreeCount / totalTrees * 100, 2) : 0;
                string description = $"Avg Air: {z.AvgAirTemp}°C, Avg Soil Moisture: {z.AvgSoilMoisture}%";
                return new EnvironmentalZoneItem(z.ZoneName, z.TreeCount, percentage, description);
            })
            .ToList();

        return items;
    }

    public async Task<List<TopTreeByMetricItem>> GetTopTreesByMetricChartAsync(
        Guid plantationId,
        string metricType,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int limit = 10,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var query = _context.Trees
            .Where(t => t.PlantationId == plantationId)
            .Select(t => new
            {
                t.Id,
                Metrics = t.Metrics
                    .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
                    .ToList()
            })
            .Where(t => t.Metrics.Any());

        var treeMetrics = await query.ToListAsync(ct);

        var items = treeMetrics
            .Select(t =>
            {
                List<double> values = metricType.ToLower() switch
                {
                    "soiltemperature" => t.Metrics.Select(m => (double)m.SoilTemperature).ToList(),
                    "soilmoisture" => t.Metrics.Select(m => (double)m.SoilMoisture).ToList(),
                    _ => t.Metrics.Select(m => (double)m.AirTemperature).ToList() // default airtemperature
                };

                return new TopTreeByMetricItem(
                    t.Id,
                    Math.Round(values.Average(), 2),
                    Math.Round(values.Min(), 2),
                    Math.Round(values.Max(), 2),
                    values.Count
                );
            })
            .OrderByDescending(t => t.AverageValue)
            .Take(limit)
            .ToList();

        return items;
    }

    public async Task<List<MonthlyHarvestComparisonItem>> GetMonthlyHarvestComparisonChartAsync(
        Guid plantationId,
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

        var monthlyData = await _context.PlantationHarvests
            .Where(h => h.PlantationId == plantationId
                && h.HarvestDate >= startDate_effective
                && h.HarvestDate <= endDate_effective)
            .GroupBy(h => new { h.HarvestDate.Year, h.HarvestDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalYieldKg = g.Sum(h => (double)h.YieldKg),
                HarvestCount = g.Count()
            })
            .OrderBy(g => g.Year)
            .ThenBy(g => g.Month)
            .ToListAsync(ct);

        return monthlyData.Select(d => new MonthlyHarvestComparisonItem(
            d.Year,
            d.Month,
            new DateTime(d.Year, d.Month, 1, 0, 0, 0, DateTimeKind.Utc).ToString("MMM yyyy"),
            d.TotalYieldKg,
            d.HarvestCount,
            d.HarvestCount > 0 ? Math.Round(d.TotalYieldKg / d.HarvestCount, 2) : 0
        )).ToList();
    }

    public async Task<List<MonthlyTreeActivityItem>> GetTreeActivityTrendChartAsync(
        Guid plantationId,
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

        List<Tree> trees = await _context.Trees
            .Where(t => t.PlantationId == plantationId)
            .Include(t => t.Metrics)
            .ToListAsync(ct);

        var months = new List<MonthlyTreeActivityItem>();
        var current = new DateTime(startDate_effective.Year, startDate_effective.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        while (current <= endDate_effective)
        {
            DateTime monthStart = current;
            DateTime monthEnd = current.AddMonths(1).AddDays(-1);

            int activeTrees = trees.Count(t => t.Metrics.Any(m =>
                m.CreatedAt >= monthStart && m.CreatedAt <= monthEnd));
            int totalTrees = trees.Count;
            int inactiveTrees = totalTrees - activeTrees;

            months.Add(new MonthlyTreeActivityItem(
                current.Year,
                current.Month,
                current.ToString("MMM yyyy"),
                activeTrees,
                inactiveTrees,
                totalTrees
            ));

            current = current.AddMonths(1);
        }

        return months;
    }

    public async Task<List<TreesByZoneComparisonItem>> GetTreesByZoneChartAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var treeEnvironmentalData = await _context.Trees
            .Where(t => t.PlantationId == plantationId)
            .Select(t => new
            {
                t.Id,
                AvgAirTemp = t.Metrics
                    .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
                    .Average(m => (double?)m.AirTemperature),
                AvgSoilMoisture = t.Metrics
                    .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
                    .Average(m => (double?)m.SoilMoisture)
            })
            .Where(t => t.AvgAirTemp.HasValue && t.AvgSoilMoisture.HasValue)
            .ToListAsync(ct);

        if (!treeEnvironmentalData.Any())
        {
            return new List<TreesByZoneComparisonItem>();
        }

        var zones = treeEnvironmentalData
            .GroupBy(t =>
            {
                double airTemp = t.AvgAirTemp!.Value;
                double soilMoisture = t.AvgSoilMoisture!.Value;

                if (airTemp < 20 && soilMoisture < 40)
                {
                    return "Cool & Dry Soil";
                }

                else if (airTemp < 20 && soilMoisture >= 40)
                {
                    return "Cool & Moist Soil";
                }

                else if (airTemp >= 20 && airTemp < 30 && soilMoisture < 40)
                {
                    return "Moderate & Dry Soil";
                }

                else if (airTemp >= 20 && airTemp < 30 && soilMoisture >= 40)
                {
                    return "Moderate & Moist Soil";
                }

                else if (airTemp >= 30 && soilMoisture < 40)
                {
                    return "Hot & Dry Soil";
                }
                else
                {
                    return "Hot & Moist Soil";
                }

            })
            .Select(g => new TreesByZoneComparisonItem(
                g.Key,
                g.Count(),
                Math.Round(g.Average(t => t.AvgAirTemp!.Value), 2),
                Math.Round(g.Average(t => t.AvgSoilMoisture!.Value), 2)
            ))
            .OrderByDescending(z => z.TreeCount)
            .ToList();

        return zones;
    }

    public async Task<List<WeeklyHarvestPerformanceItem>> GetWeeklyHarvestPerformanceChartAsync(
        Guid plantationId,
        int weeks = 12,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate = now.AddDays(-(weeks * 7));

        List<PlantationHarvest> harvests = await _context.PlantationHarvests
            .Where(h => h.PlantationId == plantationId && h.HarvestDate >= startDate && h.HarvestDate <= now)
            .ToListAsync(ct);

        var weeklyData = new List<WeeklyHarvestPerformanceItem>();
        DateTime currentWeekStart = GetWeekStart(startDate);

        while (currentWeekStart <= now)
        {
            DateTime weekEnd = currentWeekStart.AddDays(7);
            var weekHarvests = harvests.Where(h => h.HarvestDate >= currentWeekStart && h.HarvestDate < weekEnd).ToList();

            if (weekHarvests.Any())
            {
                double totalYield = weekHarvests.Sum(h => (double)h.YieldKg);
                int harvestCount = weekHarvests.Count;

                weeklyData.Add(new WeeklyHarvestPerformanceItem(
                    currentWeekStart.Year,
                    GetWeekNumber(currentWeekStart),
                    $"Week {GetWeekNumber(currentWeekStart)} ({currentWeekStart:MMM dd})",
                    currentWeekStart,
                    totalYield,
                    harvestCount,
                    Math.Round(totalYield / harvestCount, 2)
                ));
            }

            currentWeekStart = currentWeekStart.AddDays(7);
        }

        return weeklyData;
    }

    private static int GetWeekNumber(DateTime date)
    {
        CultureInfo culture = System.Globalization.CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
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

    // Histogram methods
    public Task<List<double>> GetTreeYieldDistributionDataAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        // Since harvests are at plantation level, not tree level,
        // we'll calculate yield distribution based on tree metrics activity as a proxy
        // Return empty list for now as tree-level harvest data doesn't exist
        return Task.FromResult(new List<double>());
    }

    public async Task<List<double>> GetAirTemperatureDistributionDataAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<double> temperatures = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId)
            .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .Select(m => (double)m.AirTemperature)
            .ToListAsync(ct);

        return temperatures;
    }

    public async Task<List<double>> GetSoilTemperatureDistributionDataAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<double> temperatures = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId)
            .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .Select(m => (double)m.SoilTemperature)
            .ToListAsync(ct);

        return temperatures;
    }

    public async Task<List<double>> GetSoilMoistureDistributionDataAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-30);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<double> moisture = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId)
            .Where(m => m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .Select(m => (double)m.SoilMoisture)
            .ToListAsync(ct);

        return moisture;
    }

    public Task<List<double>> GetTreeAgeDistributionDataAsync(
        Guid plantationId,
        CancellationToken ct = default)
    {
        // Trees don't have PlantedAt field in the current schema
        // Return empty list for now
        return Task.FromResult(new List<double>());
    }

    // Area chart data methods
    public async Task<List<(DateTime Date, double Yield)>> GetCumulativeHarvestYieldDataAsync(
        Guid plantationId,
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
            .Where(p => p.Id == plantationId)
            .SelectMany(p => p.Harvests)
            .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
            .OrderBy(h => h.HarvestDate)
            .Select(h => new { h.HarvestDate, Yield = (double)h.YieldKg })
            .ToListAsync(ct);

        return harvests.Select(h => (h.HarvestDate, h.Yield)).ToList();
    }

    public async Task<List<(DateTime Date, int Count)>> GetCumulativeHarvestCountDataAsync(
        Guid plantationId,
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
            .Where(p => p.Id == plantationId)
            .SelectMany(p => p.Harvests)
            .Where(h => h.HarvestDate >= startDate_effective && h.HarvestDate <= endDate_effective)
            .OrderBy(h => h.HarvestDate)
            .Select(h => h.HarvestDate)
            .ToListAsync(ct);

        return harvests.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, int Count)>> GetTreeMonitoringAdoptionDataAsync(
        Guid plantationId,
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

        // Get first metric reading date for each tree as adoption date
        List<DateTime> adoptionDates = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId
                     && m.Timestamp >= startDate_effective
                     && m.Timestamp <= endDate_effective)
            .GroupBy(m => m.TreeId)
            .Select(g => g.Min(m => m.Timestamp))
            .ToListAsync(ct);

        return adoptionDates.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, int Count)>> GetCumulativeMetricReadingsDataAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-3);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<DateTime> readings = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId
                     && m.Timestamp >= startDate_effective
                     && m.Timestamp <= endDate_effective)
            .OrderBy(m => m.Timestamp)
            .Select(m => m.Timestamp)
            .ToListAsync(ct);

        return readings.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, string MetricType, double Value)>> GetStackedMetricsByTypeDataAsync(
        Guid plantationId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-3);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var metrics = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId
                     && m.Timestamp >= startDate_effective
                     && m.Timestamp <= endDate_effective)
            .OrderBy(m => m.Timestamp)
            .Select(m => new { m.Timestamp, m.AirTemperature, m.SoilTemperature, m.SoilMoisture })
            .ToListAsync(ct);

        var result = new List<(DateTime, string, double)>();
        foreach (var m in metrics)
        {
            result.Add((m.Timestamp, "Air Temperature", (double)m.AirTemperature));
            result.Add((m.Timestamp, "Soil Temperature", (double)m.SoilTemperature));
            result.Add((m.Timestamp, "Soil Moisture", (double)m.SoilMoisture));
        }

        return result;
    }
}


