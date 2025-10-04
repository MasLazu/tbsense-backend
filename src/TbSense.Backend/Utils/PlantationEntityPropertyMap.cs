using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class PlantationEntityPropertyMap : IEntityPropertyMap<Plantation>
{
    private readonly Dictionary<string, Expression<Func<Plantation, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", p => p.Id },
            { "name", p => p.Name },
            { "description", p => p.Description },
            { "landAreaHectares", p => p.LandAreaHectares },
            { "plantedDate", p => p.PlantedDate },
            { "createdAt", p => p.CreatedAt },
            { "updatedAt", p => p.UpdatedAt! },
        };

    public Expression<Func<Plantation, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<Plantation, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for Plantation. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}