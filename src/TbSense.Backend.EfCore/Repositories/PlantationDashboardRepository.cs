using Microsoft.EntityFrameworkCore;
using TbSense.Backend.Interfaces;
using TbSense.Backend.EfCore.Data;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.EfCore.Repositories;

public class PlantationDashboardRepository : IPlantationDashboardRepository
{
    private readonly TbSenseBackendDbContext _context;

    public PlantationDashboardRepository(TbSenseBackendDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<(Guid id, string name, double landAreaHectares, DateTime plantedDate, int ageInDays, double ageInYears)?> GetPlantationBasicSummaryAsync(Guid plantationId, CancellationToken ct = default)
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

        return (plantation.Id, plantation.Name, plantation.LandAreaHectares, plantation.PlantedDate, ageInDays, ageInYears);
    }

    public async Task<(int totalTrees, double treesPerHectare, int activeTreesWithMetrics)> GetPlantationTreesSummaryAsync(Guid plantationId, CancellationToken ct = default)
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
            return (0, 0, 0);
        }

        double treesPerHectare = data.LandArea > 0 ? data.TotalTrees / (double)data.LandArea : 0;

        return (data.TotalTrees, treesPerHectare, data.ActiveTrees);
    }

    public async Task<(double totalYieldKg, double yieldPerHectare, int harvestCount, double averagePerHarvest)> GetPlantationHarvestCurrentMonthAsync(Guid plantationId, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1);
        DateTime endOfMonth = startOfMonth.AddMonths(1);

        var data = await _context.Plantations
            .Where(p => p.Id == plantationId)
            .Select(p => new
            {
                LandArea = p.LandAreaHectares,
                Harvests = p.Harvests
                    .Where(h => h.HarvestDate >= startOfMonth && h.HarvestDate < endOfMonth)
                    .Select(h => h.YieldKg)
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (data == null)
        {

            return (0, 0, 0, 0);
        }

        double totalYieldKg = data.Harvests.Sum(y => (double)y);
        int harvestCount = data.Harvests.Count;
        double yieldPerHectare = data.LandArea > 0 ? totalYieldKg / (double)data.LandArea : 0;
        double averagePerHarvest = harvestCount > 0 ? totalYieldKg / harvestCount : 0;

        return (totalYieldKg, yieldPerHectare, harvestCount, averagePerHarvest);
    }

    public async Task<(double totalYieldKg, double yieldPerHectare, int harvestCount)> GetPlantationHarvestCurrentYearAsync(Guid plantationId, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        var startOfYear = new DateTime(now.Year, 1, 1);

        var data = await _context.Plantations
            .Where(p => p.Id == plantationId)
            .Select(p => new
            {
                LandArea = p.LandAreaHectares,
                Harvests = p.Harvests
                    .Where(h => h.HarvestDate >= startOfYear)
                    .Select(h => h.YieldKg)
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (data == null)
        {
            return (0, 0, 0);
        }

        double totalYieldKg = data.Harvests.Sum(y => (double)y);
        int harvestCount = data.Harvests.Count;
        double yieldPerHectare = data.LandArea > 0 ? totalYieldKg / (double)data.LandArea : 0;

        return (totalYieldKg, yieldPerHectare, harvestCount);
    }

    public async Task<(double totalYieldKg, double averageYieldPerHectare, int totalHarvests, DateTime? firstHarvestDate)> GetPlantationHarvestAllTimeAsync(Guid plantationId, CancellationToken ct = default)
    {
        var data = await _context.Plantations
            .Where(p => p.Id == plantationId)
            .Select(p => new
            {
                LandArea = p.LandAreaHectares,
                TotalYield = p.Harvests.Sum(h => (double)h.YieldKg),
                HarvestCount = p.Harvests.Count,
                FirstHarvest = p.Harvests.OrderBy(h => h.HarvestDate).Select(h => h.HarvestDate).FirstOrDefault()
            })
            .FirstOrDefaultAsync(ct);

        if (data == null)
        {
            return (0, 0, 0, null);
        }

        double averageYieldPerHectare = data.LandArea > 0 ? data.TotalYield / (double)data.LandArea : 0;
        DateTime? firstHarvestDate = data.FirstHarvest != default ? (DateTime?)data.FirstHarvest : null;

        return (data.TotalYield, averageYieldPerHectare, data.HarvestCount, firstHarvestDate);
    }

    public async Task<(int rank, int totalPlantations, double percentile, string category)?> GetPlantationRankingAsync(Guid plantationId, DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        // Get all plantations with their yield per hectare for the period
        var plantationYields = await _context.Plantations
            .Select(p => new
            {
                p.Id,
                LandArea = p.LandAreaHectares,
                TotalYield = p.Harvests
                    .Where(h => h.HarvestDate >= startDate && h.HarvestDate < endDate)
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


        return (targetPlantation.Rank, totalPlantations, percentile, category);
    }

    public async Task<(double temperature, double soilMoisture, double humidity, double lightIntensity, DateTime lastUpdated)?> GetPlantationCurrentMetricsAsync(Guid plantationId, CancellationToken ct = default)
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

        double avgTemperature = metrics.Average(m => (double)m.AirTemperature);
        double avgSoilMoisture = metrics.Average(m => (double)m.SoilMoisture);
        double avgHumidity = 0.0; // TODO: Add when field exists
        double avgLightIntensity = 0.0; // TODO: Add when field exists
        DateTimeOffset lastUpdated = metrics.Max(m => m.CreatedAt);

        return (avgTemperature, avgSoilMoisture, avgHumidity, avgLightIntensity, lastUpdated.DateTime);
    }

    public async Task<Dictionary<string, string>> GetPlantationHealthStatusAsync(Guid plantationId, CancellationToken ct = default)
    {
        (double temperature, double soilMoisture, double humidity, double lightIntensity, DateTime lastUpdated)? metrics = await GetPlantationCurrentMetricsAsync(plantationId);

        if (metrics == null)
        {
            return new Dictionary<string, string>
            {
                ["overall"] = "unknown",
                ["temperature"] = "unknown",
                ["soilMoisture"] = "unknown",
                ["humidity"] = "unknown"
            };
        }

        string tempStatus = GetStatus(metrics.Value.temperature, 22, 28);
        string soilStatus = GetStatus(metrics.Value.soilMoisture, 40, 60);

        string overall = (tempStatus == "critical" || soilStatus == "critical") ? "critical" :
                      (tempStatus == "warning" || soilStatus == "warning") ? "warning" : "healthy";

        return new Dictionary<string, string>
        {
            ["overall"] = overall,
            ["temperature"] = tempStatus,
            ["soilMoisture"] = soilStatus,
            ["humidity"] = "unknown"
        };
    }

    public async Task<Dictionary<string, (double avg, double min, double max)>> GetPlantationEnvironmentalAveragesAsync(Guid plantationId, int days, CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate = now.AddDays(-days);

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.Tree.PlantationId == plantationId && m.CreatedAt >= startDate)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new Dictionary<string, (double avg, double min, double max)>();
        }

        var temperatures = metrics.Select(m => (double)m.AirTemperature).ToList();
        var soilMoistures = metrics.Select(m => (double)m.SoilMoisture).ToList();

        return new Dictionary<string, (double avg, double min, double max)>
        {
            ["temperature"] = (temperatures.Average(), temperatures.Min(), temperatures.Max()),
            ["soilMoisture"] = (soilMoistures.Average(), soilMoistures.Min(), soilMoistures.Max())
        };
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
