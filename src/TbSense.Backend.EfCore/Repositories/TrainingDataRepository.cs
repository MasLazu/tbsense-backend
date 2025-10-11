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
            .SelectMany(p => p.Harvests
                .Where(h => h.HarvestDate >= startDate && h.HarvestDate <= endDate)
                .Select(h => new
                {
                    Plantation = p,
                    Harvest = h,
                    PlantationAgeDays = (int)(h.HarvestDate - p.PlantedDate).TotalDays,
                    TreeCount = p.Trees.Count,
                    MetricsData = p.Trees
                        .SelectMany(t => t.Metrics
                            .Where(m => m.Timestamp >= startDate && m.Timestamp <= h.HarvestDate))
                        .GroupBy(m => 1)
                        .Select(g => new
                        {
                            AvgAirTemperature = g.Average(m => m.AirTemperature),
                            AvgSoilTemperature = g.Average(m => m.SoilTemperature),
                            AvgSoilMoisture = g.Average(m => m.SoilMoisture),
                            MetricCount = g.Count()
                        })
                        .FirstOrDefault(),
                    Yield = h.YieldKg
                }))
            .Where(x => x.TreeCount > 0 &&
                       x.MetricsData != null &&
                       x.MetricsData.MetricCount > 0 &&
                       x.Yield > 0)
            .ToListAsync(ct);

        var result = trainingData.Select(x =>
        {
            double treesPerHectare = x.Plantation.LandAreaHectares > 0
                ? x.TreeCount / (double)x.Plantation.LandAreaHectares
                : 0;

            double yieldPerHectarePerMonth = x.Plantation.LandAreaHectares > 0
                ? x.Yield / x.Plantation.LandAreaHectares / periodMonths
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
                PeriodEnd: x.Harvest.HarvestDate
            );
        }).ToList();

        return result;
    }
}
