using Microsoft.EntityFrameworkCore;
using TbSense.Backend.Interfaces;
using TbSense.Backend.EfCore.Data;
using TbSense.Backend.Domain.Entities;
using TbSense.Backend.Abstraction.Models;
using System.Globalization;

namespace TbSense.Backend.EfCore.Repositories;

public class TreeDashboardRepository : ITreeDashboardRepository
{
    private readonly TbSenseBackendDbContext _context;

    public TreeDashboardRepository(TbSenseBackendDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<TreeBasicInfoResponse?> GetTreeBasicInfoAsync(Guid treeId, CancellationToken ct = default)
    {
        var tree = await _context.Trees
            .Where(t => t.Id == treeId)
            .Select(t => new
            {
                t.Id,
                t.PlantationId,
                PlantationName = t.Plantation.Name,
                t.Latitude,
                t.Longitude,
                t.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (tree == null)
        {
            return null;
        }

        DateTime now = DateTime.UtcNow;
        int ageInDays = (int)(now - tree.CreatedAt).TotalDays;

        DateTime sevenDaysAgo = now.AddDays(-7);
        bool isActive = await _context.TreeMetrics
            .AnyAsync(m => m.TreeId == treeId && m.CreatedAt >= sevenDaysAgo, ct);

        return new TreeBasicInfoResponse(
            tree.Id,
            tree.PlantationId,
            tree.PlantationName,
            tree.Latitude,
            tree.Longitude,
            tree.CreatedAt.UtcDateTime,
            ageInDays,
            isActive
        );
    }

    public async Task<TreeCurrentMetricsResponse?> GetTreeCurrentMetricsAsync(Guid treeId, CancellationToken ct = default)
    {
        var latestMetric = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId)
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new
            {
                m.AirTemperature,
                m.SoilTemperature,
                m.SoilMoisture,
                m.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (latestMetric == null)
        {
            return null;
        }

        DateTime now = DateTime.UtcNow;
        int minutesSinceLastUpdate = (int)(now - latestMetric.CreatedAt).TotalMinutes;

        return new TreeCurrentMetricsResponse(
            latestMetric.AirTemperature,
            latestMetric.SoilTemperature,
            latestMetric.SoilMoisture,
            latestMetric.CreatedAt.UtcDateTime,
            minutesSinceLastUpdate
        );
    }

    public async Task<TreeEnvironmentalAveragesResponse> GetTreeEnvironmentalAveragesAsync(
        Guid treeId,
        int days,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate = now.AddDays(-days);

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new TreeEnvironmentalAveragesResponse(
                new EnvironmentalAverageValue(0, 0, 0),
                new EnvironmentalAverageValue(0, 0, 0),
                new EnvironmentalAverageValue(0, 0, 0),
                0
            );
        }

        var airTemperatures = metrics.Select(m => (double)m.AirTemperature).ToList();
        var soilTemperatures = metrics.Select(m => (double)m.SoilTemperature).ToList();
        var soilMoistures = metrics.Select(m => (double)m.SoilMoisture).ToList();

        return new TreeEnvironmentalAveragesResponse(
            new EnvironmentalAverageValue(airTemperatures.Average(), airTemperatures.Min(), airTemperatures.Max()),
            new EnvironmentalAverageValue(soilTemperatures.Average(), soilTemperatures.Min(), soilTemperatures.Max()),
            new EnvironmentalAverageValue(soilMoistures.Average(), soilMoistures.Min(), soilMoistures.Max()),
            metrics.Count
        );
    }

    public async Task<List<TreeEnvironmentalTimeseriesDataPoint>> GetTreeEnvironmentalTimeseriesAsync(
        Guid treeId,
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
            .Where(m => m.TreeId == treeId &&
                       m.CreatedAt >= startDate_effective &&
                       m.CreatedAt <= endDate_effective)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new List<TreeEnvironmentalTimeseriesDataPoint>();
        }

        List<TreeEnvironmentalTimeseriesDataPoint> dataPoints = interval.ToLower() switch
        {
            "hourly" => metrics
                .GroupBy(m => new DateTime(m.CreatedAt.Year, m.CreatedAt.Month, m.CreatedAt.Day, m.CreatedAt.Hour, 0, 0, DateTimeKind.Utc))
                .Select(g => new TreeEnvironmentalTimeseriesDataPoint(
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
                .Select(g => new TreeEnvironmentalTimeseriesDataPoint(
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
                .Select(g => new TreeEnvironmentalTimeseriesDataPoint(
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
                .Select(g => new TreeEnvironmentalTimeseriesDataPoint(
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

    private static DateTime GetWeekStart(DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc).AddDays(-diff);
    }

    public async Task<List<ReadingDistributionItem>> GetTreeReadingDistributionChartAsync(
        Guid treeId,
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

        var readings = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId
                && m.CreatedAt >= startDate_effective
                && m.CreatedAt <= endDate_effective)
            .GroupBy(m => m.CreatedAt.Hour)
            .Select(g => new
            {
                Hour = g.Key,
                Count = g.Count()
            })
            .OrderBy(g => g.Hour)
            .ToListAsync(ct);

        int totalReadings = readings.Sum(r => r.Count);

        var items = readings
            .Select(r =>
            {
                string timeOfDay = GetTimeOfDayLabel(r.Hour);
                double percentage = totalReadings > 0 ? Math.Round((double)r.Count / totalReadings * 100, 2) : 0;
                return new ReadingDistributionItem(timeOfDay, r.Count, percentage);
            })
            .ToList();

        return items;
    }

    public async Task<List<MetricRangeItem>> GetTreeMetricRangesChartAsync(
        Guid treeId,
        string metricType,
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

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId
                && m.CreatedAt >= startDate_effective
                && m.CreatedAt <= endDate_effective)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new List<MetricRangeItem>();
        }

        List<float> values = metricType.ToLower() switch
        {
            "airtemperature" => metrics.Select(m => m.AirTemperature).ToList(),
            "soiltemperature" => metrics.Select(m => m.SoilTemperature).ToList(),
            "soilmoisture" => metrics.Select(m => m.SoilMoisture).ToList(),
            _ => metrics.Select(m => m.AirTemperature).ToList() // default to air temperature
        };

        float minValue = values.Min();
        float maxValue = values.Max();
        float range = maxValue - minValue;

        if (range == 0)
        {
            return new List<MetricRangeItem>
            {
                new MetricRangeItem($"{minValue:F1}", values.Count, 100.0)
            };
        }

        // Create 5 ranges
        int numRanges = 5;
        float rangeSize = range / numRanges;

        var ranges = Enumerable.Range(0, numRanges)
            .Select(i =>
            {
                float rangeStart = minValue + i * rangeSize;
                float rangeEnd = i == numRanges - 1 ? maxValue : rangeStart + rangeSize;
                int count = values.Count(v => v >= rangeStart && v < rangeEnd || i == numRanges - 1 && v == rangeEnd);
                string rangeLabel = $"{rangeStart:F1} - {rangeEnd:F1}";
                double percentage = Math.Round((double)count / values.Count * 100, 2);
                return new { RangeLabel = rangeLabel, Count = count, Percentage = percentage };
            })
            .Where(r => r.Count > 0)
            .OrderBy(r => r.RangeLabel)
            .ToList();

        var items = ranges
            .Select(r => new MetricRangeItem(r.RangeLabel, r.Count, r.Percentage))
            .ToList();

        return items;
    }

    public async Task<List<DailyMetricComparisonItem>> GetDailyMetricsComparisonChartAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-7);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .ToListAsync(ct);

        var items = metrics
            .GroupBy(m => new DateTime(m.CreatedAt.Year, m.CreatedAt.Month, m.CreatedAt.Day, 0, 0, 0, DateTimeKind.Utc))
            .Select(g => new DailyMetricComparisonItem(
                g.Key,
                Math.Round(g.Average(m => (double)m.AirTemperature), 2),
                Math.Round(g.Average(m => (double)m.SoilTemperature), 2),
                Math.Round(g.Average(m => (double)m.SoilMoisture), 2),
                g.Count()
            ))
            .OrderBy(i => i.Date)
            .ToList();

        return items;
    }

    public async Task<List<HourlyAverageComparisonItem>> GetHourlyAverageComparisonChartAsync(
        Guid treeId,
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

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .ToListAsync(ct);

        var items = metrics
            .GroupBy(m => m.CreatedAt.Hour)
            .Select(g => new HourlyAverageComparisonItem(
                g.Key,
                $"{g.Key:D2}:00",
                Math.Round(g.Average(m => (double)m.AirTemperature), 2),
                Math.Round(g.Average(m => (double)m.SoilTemperature), 2),
                Math.Round(g.Average(m => (double)m.SoilMoisture), 2),
                g.Count()
            ))
            .OrderBy(i => i.Hour)
            .ToList();

        return items;
    }

    public async Task<List<WeeklyMetricComparisonItem>> GetWeeklyMetricsComparisonChartAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddDays(-56); // 8 weeks
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .ToListAsync(ct);

        var items = metrics
            .GroupBy(m => GetWeekStart(m.CreatedAt.UtcDateTime))
            .Select(g => new WeeklyMetricComparisonItem(
                g.Key.Year,
                GetWeekNumber(g.Key),
                $"Week {GetWeekNumber(g.Key)} ({g.Key:MMM dd})",
                g.Key,
                Math.Round(g.Average(m => (double)m.AirTemperature), 2),
                Math.Round(g.Average(m => (double)m.SoilTemperature), 2),
                Math.Round(g.Average(m => (double)m.SoilMoisture), 2),
                g.Count()
            ))
            .OrderBy(i => i.WeekStartDate)
            .ToList();

        return items;
    }

    public async Task<List<MetricRangeComparisonItem>> GetMetricRangeComparisonChartAsync(
        Guid treeId,
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

        List<TreeMetric> metrics = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .ToListAsync(ct);

        if (!metrics.Any())
        {
            return new List<MetricRangeComparisonItem>();
        }

        var items = new List<MetricRangeComparisonItem>
        {
            new MetricRangeComparisonItem(
                "Air Temperature",
                Math.Round(metrics.Min(m => (double)m.AirTemperature), 2),
                Math.Round(metrics.Average(m => (double)m.AirTemperature), 2),
                Math.Round(metrics.Max(m => (double)m.AirTemperature), 2),
                metrics.Count
            ),
            new MetricRangeComparisonItem(
                "Soil Temperature",
                Math.Round(metrics.Min(m => (double)m.SoilTemperature), 2),
                Math.Round(metrics.Average(m => (double)m.SoilTemperature), 2),
                Math.Round(metrics.Max(m => (double)m.SoilTemperature), 2),
                metrics.Count
            ),
            new MetricRangeComparisonItem(
                "Soil Moisture",
                Math.Round(metrics.Min(m => (double)m.SoilMoisture), 2),
                Math.Round(metrics.Average(m => (double)m.SoilMoisture), 2),
                Math.Round(metrics.Max(m => (double)m.SoilMoisture), 2),
                metrics.Count
            )
        };

        return items;
    }

    private static int GetWeekNumber(DateTime date)
    {
        CultureInfo culture = CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }

    private static string GetTimeOfDayLabel(int hour)
    {
        return hour switch
        {
            >= 0 and < 6 => "Night (00:00-05:59)",
            >= 6 and < 12 => "Morning (06:00-11:59)",
            >= 12 and < 18 => "Afternoon (12:00-17:59)",
            _ => "Evening (18:00-23:59)"
        };
    }

    // Histogram methods
    public async Task<List<double>> GetAirTemperatureDistributionDataAsync(
        Guid treeId,
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
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .Select(m => (double)m.AirTemperature)
            .ToListAsync(ct);

        return temperatures;
    }

    public async Task<List<double>> GetSoilTemperatureDistributionDataAsync(
        Guid treeId,
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
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .Select(m => (double)m.SoilTemperature)
            .ToListAsync(ct);

        return temperatures;
    }

    public async Task<List<double>> GetSoilMoistureDistributionDataAsync(
        Guid treeId,
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
            .Where(m => m.TreeId == treeId && m.CreatedAt >= startDate_effective && m.CreatedAt <= endDate_effective)
            .Select(m => (double)m.SoilMoisture)
            .ToListAsync(ct);

        return moisture;
    }

    // Area chart data methods
    public async Task<List<(DateTime Date, int Count)>> GetCumulativeMetricReadingsDataAsync(
        Guid treeId,
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
            .Where(m => m.TreeId == treeId
                     && m.Timestamp >= startDate_effective
                     && m.Timestamp <= endDate_effective)
            .OrderBy(m => m.Timestamp)
            .Select(m => m.Timestamp)
            .ToListAsync(ct);

        return readings.Select(date => (date, 1)).ToList();
    }

    public async Task<List<(DateTime Date, string MetricType, double Value)>> GetStackedMetricsByTypeDataAsync(
        Guid treeId,
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
            .Where(m => m.TreeId == treeId
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

    public async Task<List<(DateTime Date, double Temperature)>> GetCumulativeTemperatureExposureDataAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        double threshold = 30.0,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-3);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var temperatures = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId
                     && m.Timestamp >= startDate_effective
                     && m.Timestamp <= endDate_effective
                     && m.AirTemperature > (float)threshold)
            .OrderBy(m => m.Timestamp)
            .Select(m => new { m.Timestamp, Temp = (double)m.AirTemperature })
            .ToListAsync(ct);

        return temperatures.Select(t => (t.Timestamp, t.Temp - threshold)).ToList();
    }

    public async Task<List<(DateTime Date, double Moisture)>> GetCumulativeMoistureDeficitDataAsync(
        Guid treeId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        double threshold = 30.0,
        CancellationToken ct = default)
    {
        DateTime now = DateTime.UtcNow;
        DateTime startDate_effective = startDate.HasValue
            ? DateTime.SpecifyKind(startDate.Value, DateTimeKind.Utc)
            : now.AddMonths(-3);
        DateTime endDate_effective = endDate.HasValue
            ? DateTime.SpecifyKind(endDate.Value, DateTimeKind.Utc)
            : now;

        var moistureLevels = await _context.TreeMetrics
            .Where(m => m.TreeId == treeId
                     && m.Timestamp >= startDate_effective
                     && m.Timestamp <= endDate_effective
                     && m.SoilMoisture < (float)threshold)
            .OrderBy(m => m.Timestamp)
            .Select(m => new { m.Timestamp, Moisture = (double)m.SoilMoisture })
            .ToListAsync(ct);

        return moistureLevels.Select(m => (m.Timestamp, threshold - m.Moisture)).ToList();
    }
}


