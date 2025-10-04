using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class PlantationCoordinateEntityPropertyMap : IEntityPropertyMap<PlantationCoordinate>
{
    private readonly Dictionary<string, Expression<Func<PlantationCoordinate, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", pc => pc.Id },
            { "plantationId", pc => pc.PlantationId },
            { "longitude", pc => pc.Longitude },
            { "latitude", pc => pc.Latitude },
            { "createdAt", pc => pc.CreatedAt },
            { "updatedAt", pc => pc.UpdatedAt! },
        };

    public Expression<Func<PlantationCoordinate, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<PlantationCoordinate, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for PlantationCoordinate. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}