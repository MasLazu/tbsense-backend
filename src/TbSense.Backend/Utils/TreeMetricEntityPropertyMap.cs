using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class TreeMetricEntityPropertyMap : IEntityPropertyMap<TreeMetric>
{
    private readonly Dictionary<string, Expression<Func<TreeMetric, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", tm => tm.Id },
            { "treeId", tm => tm.TreeId },
            { "soilMoisture", tm => tm.SoilMoisture },
            { "soilTemperature", tm => tm.SoilTemperature },
            { "airTemperature", tm => tm.AirTemperature },
            { "timestamp", tm => tm.Timestamp },
            { "createdAt", tm => tm.CreatedAt },
            { "updatedAt", tm => tm.UpdatedAt! },
        };

    public Expression<Func<TreeMetric, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<TreeMetric, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for TreeMetric. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}