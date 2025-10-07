using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class KnowledgeBaseEntityPropertyMap : IEntityPropertyMap<KnowledgeBase>
{
    private readonly Dictionary<string, Expression<Func<KnowledgeBase, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", k => k.Id },
            { "title", k => k.Title },
            { "content", k => k.Content },
            { "createdAt", k => k.CreatedAt },
            { "updatedAt", k => k.UpdatedAt! },
        };

    public Expression<Func<KnowledgeBase, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<KnowledgeBase, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for KnowledgeBase. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}