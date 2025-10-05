using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Utils;

public static class AreaChartHelper
{
    public static List<AreaChartDataPoint> CreateSimpleAreaChart(
        List<(DateTime Date, double Value)> rawData,
        string interval = "daily")
    {
        if (rawData.Count == 0)
        {
            return new List<AreaChartDataPoint>();
        }

        // Group by interval
        List<(DateTime Date, double Value)> grouped = GroupByInterval(
            rawData.Select(d => (d.Date, d.Value, d.Value)).ToList(),
            interval);

        // Calculate cumulative values
        double cumulative = 0;
        var result = new List<AreaChartDataPoint>();

        foreach ((DateTime Date, double Value) group in grouped.OrderBy(g => g.Date))
        {
            cumulative += group.Value;
            result.Add(new AreaChartDataPoint(group.Date, group.Value, cumulative));
        }

        return result;
    }

    public static List<AreaChartDataPoint> CreateCountAreaChart(
        List<(DateTime Date, int Count)> rawData,
        string interval = "daily")
    {
        if (rawData.Count == 0)
        {
            return new List<AreaChartDataPoint>();
        }

        // Group by interval and sum counts
        List<(DateTime Date, double Value)> grouped = GroupByInterval(
            rawData.Select(d => (d.Date, (double)d.Count, (double)d.Count)).ToList(),
            interval);

        // Calculate cumulative values
        double cumulative = 0;
        var result = new List<AreaChartDataPoint>();

        foreach ((DateTime Date, double Value) group in grouped.OrderBy(g => g.Date))
        {
            cumulative += group.Value;
            result.Add(new AreaChartDataPoint(group.Date, group.Value, cumulative));
        }

        return result;
    }

    public static List<StackedAreaChartDataPoint> CreateStackedAreaChart(
        List<(DateTime Date, string Category, double Value)> rawData,
        string interval = "daily")
    {
        if (rawData.Count == 0)
        {
            return new List<StackedAreaChartDataPoint>();
        }

        // Group by interval and category
        var grouped = rawData
            .GroupBy(d => new { IntervalDate = GetIntervalDate(d.Date, interval), d.Category })
            .Select(g => new
            {
                g.Key.IntervalDate,
                g.Key.Category,
                Value = g.Sum(x => x.Value)
            })
            .ToList();

        // Get all unique dates and categories
        var dates = grouped.Select(g => g.IntervalDate).Distinct().OrderBy(d => d).ToList();
        var categories = grouped.Select(g => g.Category).Distinct().ToList();

        // Initialize cumulative tracking per category
        var cumulatives = categories.ToDictionary(c => c, c => 0.0);
        var result = new List<StackedAreaChartDataPoint>();

        foreach (DateTime date in dates)
        {
            var dayData = grouped.Where(g => g.IntervalDate == date).ToList();
            var values = new Dictionary<string, double>();

            foreach (string? category in categories)
            {
                double value = dayData.FirstOrDefault(d => d.Category == category)?.Value ?? 0;
                cumulatives[category] += value;
                values[category] = cumulatives[category];
            }

            double totalCumulative = cumulatives.Values.Sum();
            result.Add(new StackedAreaChartDataPoint(date, values, totalCumulative));
        }

        return result;
    }

    private static List<(DateTime Date, double Value)> GroupByInterval(
        List<(DateTime Date, double Value, double OriginalValue)> data,
        string interval)
    {
        return data
            .GroupBy(d => GetIntervalDate(d.Date, interval))
            .Select(g => (g.Key, g.Sum(x => x.Value)))
            .OrderBy(x => x.Key)
            .ToList();
    }

    private static DateTime GetIntervalDate(DateTime date, string interval)
    {
        return interval.ToLower() switch
        {
            "daily" => date.Date,
            "weekly" => date.Date.AddDays(-(int)date.DayOfWeek),
            "monthly" => new DateTime(date.Year, date.Month, 1),
            _ => date.Date
        };
    }

    public static double CalculateAverageIncrease(List<AreaChartDataPoint> data, string interval)
    {
        if (data.Count <= 1)
        {
            return 0;
        }

        double totalIncrease = data.Last().CumulativeValue - data.First().Value;
        int periods = data.Count - 1;

        return periods > 0 ? totalIncrease / periods : 0;
    }

    public static double CalculateAverageIncreaseForStacked(List<StackedAreaChartDataPoint> data, string interval)
    {
        if (data.Count <= 1)
        {
            return 0;
        }

        double firstTotal = data.First().TotalCumulative;
        double lastTotal = data.Last().TotalCumulative;
        double totalIncrease = lastTotal - firstTotal;
        int periods = data.Count - 1;

        return periods > 0 ? totalIncrease / periods : 0;
    }
}
