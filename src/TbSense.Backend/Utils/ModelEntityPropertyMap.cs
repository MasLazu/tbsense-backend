using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using TbSense.Backend.Domain.Entities;

namespace TbSense.Backend.Utils;

public class ModelEntityPropertyMap : IEntityPropertyMap<Model>
{
    private readonly Dictionary<string, Expression<Func<Model, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", m => m.Id },
            { "trainingStatus", m => m.TrainingStatus },
            { "filePath", m => m.FilePath },
            { "isUsed", m => m.IsUsed },
            { "trainingDataStart", m => m.TrainingDataStart },
            { "trainingDataEnd", m => m.TrainingDataEnd },
            { "accuracy", m => m.Accuracy! },
            { "mae", m => m.MAE! },
            { "rmse", m => m.RMSE! },
            { "r2Score", m => m.R2Score! },
            { "createdAt", m => m.CreatedAt },
            { "updatedAt", m => m.UpdatedAt! },
        };

    public Expression<Func<Model, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<Model, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for Model. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}