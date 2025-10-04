using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class PlantationYieldPredictionEntityPropertyMap : IEntityPropertyMap<PlantationYieldPrediction>
{
    private readonly Dictionary<string, Expression<Func<PlantationYieldPrediction, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", pyp => pyp.Id },
            { "plantationId", pyp => pyp.PlantationId },
            { "modelId", pyp => pyp.ModelId },
            { "timestamp", pyp => pyp.Timestamp },
            { "createdAt", pyp => pyp.CreatedAt },
            { "updatedAt", pyp => pyp.UpdatedAt! },
        };

    public Expression<Func<PlantationYieldPrediction, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<PlantationYieldPrediction, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for PlantationYieldPrediction. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}