using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class TreeEntityPropertyMap : IEntityPropertyMap<Tree>
{
    private readonly Dictionary<string, Expression<Func<Tree, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", t => t.Id },
            { "plantationId", t => t.PlantationId },
            { "longitude", t => t.Longitude },
            { "latitude", t => t.Latitude },
            { "createdAt", t => t.CreatedAt },
            { "updatedAt", t => t.UpdatedAt! },
        };

    public Expression<Func<Tree, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<Tree, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for Tree. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}