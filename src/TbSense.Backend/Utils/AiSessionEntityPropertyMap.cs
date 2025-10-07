using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class AiSessionEntityPropertyMap : IEntityPropertyMap<AiSession>
{
    private readonly Dictionary<string, Expression<Func<AiSession, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", a => a.Id },
            { "title", a => a.Title },
            { "lastActivityAt", a => a.LastActivityAt },
            { "createdAt", a => a.CreatedAt },
            { "updatedAt", a => a.UpdatedAt! },
        };

    public Expression<Func<AiSession, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<AiSession, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for AiSession. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}