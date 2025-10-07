using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class SystemPromptEntityPropertyMap : IEntityPropertyMap<SystemPrompt>
{
    private readonly Dictionary<string, Expression<Func<SystemPrompt, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", s => s.Id },
            { "prompt", s => s.Prompt },
            { "createdAt", s => s.CreatedAt },
            { "updatedAt", s => s.UpdatedAt! },
        };

    public Expression<Func<SystemPrompt, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<SystemPrompt, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for SystemPrompt. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}