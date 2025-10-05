using Microsoft.EntityFrameworkCore;
using TbSense.Backend.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.EfCore.Data;

namespace TbSense.Backend.EfCore.Repositories;

public class TrainingDataRepository : ITrainingDataRepository
{
    private readonly TbSenseBackendDbContext _context;

    public TrainingDataRepository(TbSenseBackendDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<ProcessedDataRow>> GetTrainingDataAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default)
    {
        double periodMonths = (endDate - startDate).TotalDays / 30.0;
        if (periodMonths == 0)
        {
            periodMonths = 1;
        }

        var trainingData = await _context.Plantations
            .Where(p => p.PlantedDate <= startDate)
            .Select(p => new
            {
                Plantation = p,
                PlantationAgeDays = (int)(startDate - p.PlantedDate).TotalDays,
                TreeCount = p.Trees.Count,
                MetricsData = p.Trees
                    .SelectMany(t => t.Metrics
                        .Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate))
                    .GroupBy(m => 1)
                    .Select(g => new
                    {
                        AvgAirTemperature = g.Average(m => m.AirTemperature),
                        AvgSoilTemperature = g.Average(m => m.SoilTemperature),
                        AvgSoilMoisture = g.Average(m => m.SoilMoisture),
                        MetricCount = g.Count()
                    })
                    .FirstOrDefault(),
                TotalYield = p.Harvests
                    .Where(h => h.HarvestDate >= startDate && h.HarvestDate <= endDate)
                    .Sum(h => (double?)h.YieldKg) ?? 0
            })
            .Where(x => x.TreeCount > 0 &&
                       x.MetricsData != null &&
                       x.MetricsData.MetricCount > 0 &&
                       x.TotalYield > 0)
            .ToListAsync(ct);

        var result = trainingData.Select(x =>
        {
            double treesPerHectare = x.Plantation.LandAreaHectares > 0
                ? x.TreeCount / (double)x.Plantation.LandAreaHectares
                : 0;

            double yieldPerHectarePerMonth = x.Plantation.LandAreaHectares > 0
                ? x.TotalYield / x.Plantation.LandAreaHectares / periodMonths
                : 0;

            return new ProcessedDataRow(
                PlantationId: x.Plantation.Id,
                PlantationAgeDays: x.PlantationAgeDays,
                AvgTemperature: x.MetricsData?.AvgAirTemperature ?? 0,
                AvgHumidity: 0,
                AvgSoilMoisture: x.MetricsData?.AvgSoilMoisture ?? 0,
                AvgLightIntensity: 0,
                TreesPerHectare: treesPerHectare,
                YieldPerHectarePerMonth: yieldPerHectarePerMonth,
                PeriodStart: startDate,
                PeriodEnd: endDate
            );
        }).ToList();

        return result;
    }
}
