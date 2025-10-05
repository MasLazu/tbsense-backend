using TbSense.Backend.Abstraction.Models;

namespace TbSense.Backend.Utils;

public static class HistogramHelper
{
    /// <summary>
    /// Creates histogram bins from a list of values
    /// </summary>
    /// <param name="values">The values to bin</param>
    /// <param name="binCount">Number of bins to create</param>
    /// <param name="formatPattern">Format pattern for range labels (e.g., "0.##", "0.0")</param>
    /// <param name="unit">Unit suffix for labels (e.g., "kg", "°C", "%", "hectares")</param>
    /// <returns>List of histogram bin items</returns>
    public static List<HistogramBinItem> CreateHistogramBins(
        List<double> values,
        int binCount,
        string formatPattern = "0.##",
        string unit = "")
    {
        if (values == null || values.Count == 0)
        {
            return new List<HistogramBinItem>();
        }

        double minValue = values.Min();
        double maxValue = values.Max();

        // Handle case where all values are the same
        if (Math.Abs(maxValue - minValue) < 0.0001)
        {
            return new List<HistogramBinItem>
            {
                new HistogramBinItem(
                    minValue,
                    maxValue,
                    $"{minValue.ToString(formatPattern)}{unit}",
                    values.Count,
                    100.0
                )
            };
        }

        double binWidth = (maxValue - minValue) / binCount;
        var bins = new List<HistogramBinItem>();

        for (int i = 0; i < binCount; i++)
        {
            double rangeStart = minValue + i * binWidth;
            double rangeEnd = i == binCount - 1 ? maxValue : rangeStart + binWidth;

            // Count values in this bin
            int count = values.Count(v =>
                v >= rangeStart && (i == binCount - 1 ? v <= rangeEnd : v < rangeEnd));

            double percentage = values.Count > 0
                ? Math.Round(count * 100.0 / values.Count, 2)
                : 0;

            string label = string.IsNullOrWhiteSpace(unit)
                ? $"{rangeStart.ToString(formatPattern)} - {rangeEnd.ToString(formatPattern)}"
                : $"{rangeStart.ToString(formatPattern)}-{rangeEnd.ToString(formatPattern)} {unit}";

            bins.Add(new HistogramBinItem(
                Math.Round(rangeStart, 2),
                Math.Round(rangeEnd, 2),
                label,
                count,
                percentage
            ));
        }

        return bins;
    }

    /// <summary>
    /// Calculates statistics for histogram response
    /// </summary>
    public static (double min, double max, double average, double median) CalculateStatistics(List<double> values)
    {
        if (values == null || values.Count == 0)
        {
            return (0, 0, 0, 0);
        }

        double min = values.Min();
        double max = values.Max();
        double average = Math.Round(values.Average(), 2);

        // Calculate median
        var sorted = values.OrderBy(v => v).ToList();
        double median;
        int count = sorted.Count;
        median = count % 2 == 0 ? (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0 : sorted[count / 2];
        median = Math.Round(median, 2);

        return (min, max, average, median);
    }
}
