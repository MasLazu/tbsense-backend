using Microsoft.EntityFrameworkCore;
using TbSense.Backend.Interfaces;
using TbSense.Backend.EfCore.Data;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly TbSenseBackendDbContext _context;

    public DashboardRepository(TbSenseBackendDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<(int total, int active)> GetPlantationsSummaryAsync(CancellationToken ct = default)
    {
        int total = await _context.Plantations.CountAsync(ct);
        // Consider a plantation active if it has recent metrics (last 7 days) or recent harvests (last 30 days)
        DateTime now = DateTime.UtcNow;
        DateTime sevenDaysAgo = now.AddDays(-7);
        DateTime thirtyDaysAgo = now.AddDays(-30);

        int active = await _context.Plantations
            .Where(p => p.Trees.Any(t => t.Metrics.Any(m => m.CreatedAt >= sevenDaysAgo)) ||
                       p.Harvests.Any(h => h.HarvestDate >= thirtyDaysAgo))
            .CountAsync(ct);

        return (total, active);
    }

    public async Task<(int total, double averagePerHectare)> GetTreesSummaryAsync(CancellationToken ct = default)
    {
        var data = await _context.Plantations
            .Where(p => p.LandAreaHectares > 0)
            .Select(p => new
            {
                TreeCount = p.Trees.Count,
                LandArea = p.LandAreaHectares
            })
            .ToListAsync(ct);

        int total = data.Sum(d => d.TreeCount);
        double totalLandArea = data.Sum(d => (double)d.LandArea);
        double averagePerHectare = totalLandArea > 0 ? total / totalLandArea : 0;

        return (total, averagePerHectare);
    }

    public async Task<(double totalHectares, double utilized, double utilizationRate)> GetLandAreaSummaryAsync(CancellationToken ct = default)
    {
        double totalHectares = await _context.Plantations
            .SumAsync(p => (double)p.LandAreaHectares, ct);

        // Consider land utilized if plantation has trees
        double utilized = await _context.Plantations
            .Where(p => p.Trees.Any())
            .SumAsync(p => (double)p.LandAreaHectares, ct);

        double utilizationRate = totalHectares > 0 ? utilized / totalHectares * 100 : 0;

        return (totalHectares, utilized, utilizationRate);
    }

    public async Task<(double totalYieldKg, double averageYieldPerHectare, int harvestCount)> GetHarvestCurrentMonthAsync(CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        DateTime endOfMonth = startOfMonth.AddMonths(1);

        var harvests = await _context.PlantationHarvests
            .Where(h => h.HarvestDate >= startOfMonth && h.HarvestDate < endOfMonth)
            .Select(h => new { h.YieldKg, h.Plantation.LandAreaHectares })
            .ToListAsync(ct);

        double totalYieldKg = harvests.Sum(h => (double)h.YieldKg);
        int harvestCount = harvests.Count;

        double totalLandArea = await _context.Plantations
            .SumAsync(p => (double)p.LandAreaHectares, ct);

        double averageYieldPerHectare = totalLandArea > 0 ? totalYieldKg / totalLandArea : 0;

        return (totalYieldKg, averageYieldPerHectare, harvestCount);
    }

    public async Task<(double totalYieldKg, double averageYieldPerHectare, int harvestCount)> GetHarvestCurrentYearAsync(CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        var startOfYear = new DateTime(now.Year, 1, 1);

        var harvests = await _context.PlantationHarvests
            .Where(h => h.HarvestDate >= startOfYear)
            .Select(h => new { h.YieldKg, h.Plantation.LandAreaHectares })
            .ToListAsync(ct);

        double totalYieldKg = harvests.Sum(h => (double)h.YieldKg);
        int harvestCount = harvests.Count;

        double totalLandArea = await _context.Plantations
            .SumAsync(p => (double)p.LandAreaHectares, ct);

        double averageYieldPerHectare = totalLandArea > 0 ? totalYieldKg / totalLandArea : 0;

        return (totalYieldKg, averageYieldPerHectare, harvestCount);
    }

    public async Task<Dictionary<string, (double current, double[] optimal, string status)>> GetEnvironmentalAveragesAsync(CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime oneDayAgo = now.AddDays(-1);

        // Get recent metrics (last 24 hours)
        List<TreeMetric> recentMetrics = await _context.TreeMetrics
            .Where(m => m.CreatedAt >= oneDayAgo)
            .ToListAsync(ct);

        if (!recentMetrics.Any())
        {
            return new Dictionary<string, (double current, double[] optimal, string status)>();
        }

        double avgTemperature = recentMetrics.Average(m => (double)m.AirTemperature);
        double avgSoilMoisture = recentMetrics.Average(m => (double)m.SoilMoisture);
        double avgHumidity = 0.0; // TODO: Add when field exists
        double avgLightIntensity = 0.0; // TODO: Add when field exists

        var result = new Dictionary<string, (double current, double[] optimal, string status)>
        {
            ["temperature"] = (avgTemperature, new double[] { 22, 28 }, GetStatus(avgTemperature, 22, 28)),
            ["soilMoisture"] = (avgSoilMoisture, new double[] { 40, 60 }, GetStatus(avgSoilMoisture, 40, 60)),
            ["humidity"] = (avgHumidity, new double[] { 60, 80 }, "unknown"),
            ["lightIntensity"] = (avgLightIntensity, new double[] { 400, 600 }, "unknown")
        };

        return result;
    }

    public async Task<(int plantationsInOptimalRange, int plantationsWithWarnings, int plantationsCritical, int total)> GetEnvironmentalStatusAsync(CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime oneDayAgo = now.AddDays(-1);

        int total = await _context.Plantations.CountAsync(ct);

        // Get plantations with their recent average metrics
        var plantationMetrics = await _context.Plantations
            .Select(p => new
            {
                PlantationId = p.Id,
                RecentMetrics = p.Trees
                    .SelectMany(t => t.Metrics.Where(m => m.CreatedAt >= oneDayAgo))
                    .ToList()
            })
            .ToListAsync(ct);

        int optimal = 0;
        int warnings = 0;
        int critical = 0;

        foreach (var pm in plantationMetrics)
        {
            if (!pm.RecentMetrics.Any())
            {
                warnings++; // No recent data is a warning
                continue;
            }

            double avgTemp = pm.RecentMetrics.Average(m => (double)m.AirTemperature);
            double avgSoilMoisture = pm.RecentMetrics.Average(m => (double)m.SoilMoisture);

            bool tempOptimal = avgTemp >= 22 && avgTemp <= 28;
            bool soilOptimal = avgSoilMoisture >= 40 && avgSoilMoisture <= 60;

            bool tempCritical = avgTemp < 18 || avgTemp > 32;
            bool soilCritical = avgSoilMoisture < 30 || avgSoilMoisture > 70;

            if (tempCritical || soilCritical)
            {
                critical++;
            }
            else if (!tempOptimal || !soilOptimal)
            {
                warnings++;
            }
            else
            {
                optimal++;
            }
        }

        return (optimal, warnings, critical, total);
    }

    private static string GetStatus(double value, double min, double max)
    {
        if (value >= min && value <= max)
        {
            return "optimal";
        }

        if (value < min - 5 || value > max + 5)
        {
            return "critical";
        }

        return "warning";
    }
}
