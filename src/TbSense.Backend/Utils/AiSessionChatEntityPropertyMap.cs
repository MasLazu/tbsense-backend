using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class AiSessionChatEntityPropertyMap : IEntityPropertyMap<AiSessionChat>
{
    private readonly Dictionary<string, Expression<Func<AiSessionChat, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", c => c.Id },
            { "sessionId", c => c.SessionId },
            { "role", c => c.Role },
            { "order", c => c.Order },
            { "content", c => c.Content },
            { "createdAt", c => c.CreatedAt },
            { "updatedAt", c => c.UpdatedAt! },
        };

    public Expression<Func<AiSessionChat, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<AiSessionChat, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for AiSessionChat. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}