using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreatePlantationHarvestRequest(
    Guid PlantationId,
    float YieldKg,
    DateTime HarvestDate
);

public record PlantationHarvestDto(
    Guid Id,
    Guid PlantationId,
    float YieldKg,
    DateTime HarvestDate,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdatePlantationHarvestRequest(
    Guid Id,
    Guid? PlantationId,
    float? YieldKg,
    DateTime? HarvestDate
) : BaseUpdateRequest(Id);