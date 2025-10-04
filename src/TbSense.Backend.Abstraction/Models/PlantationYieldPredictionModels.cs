using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreatePlantationYieldPredictionRequest(
    Guid PlantationId,
    Guid ModelId,
    DateTime Timestamp
);

public record PlantationYieldPredictionDto(
    Guid Id,
    Guid PlantationId,
    Guid ModelId,
    DateTime Timestamp,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdatePlantationYieldPredictionRequest(
    Guid Id,
    Guid? PlantationId,
    Guid? ModelId,
    DateTime? Timestamp
) : BaseUpdateRequest(Id);