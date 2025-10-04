using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class PlantationHarvestEntityPropertyMap : IEntityPropertyMap<PlantationHarvest>
{
    private readonly Dictionary<string, Expression<Func<PlantationHarvest, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", ph => ph.Id },
            { "plantationId", ph => ph.PlantationId },
            { "yieldKg", ph => ph.YieldKg },
            { "harvestDate", ph => ph.HarvestDate },
            { "createdAt", ph => ph.CreatedAt },
            { "updatedAt", ph => ph.UpdatedAt! },
        };

    public Expression<Func<PlantationHarvest, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<PlantationHarvest, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for PlantationHarvest. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}