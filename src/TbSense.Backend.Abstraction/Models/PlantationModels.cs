using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreatePlantationRequest(
    string Name,
    string? Description,
    float LandAreaHectares,
    DateTime PlantedDate
);

public record PlantationDto(
    Guid Id,
    string Name,
    string? Description,
    float LandAreaHectares,
    DateTime PlantedDate,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdatePlantationRequest(
    Guid Id,
    string? Name,
    string? Description,
    float? LandAreaHectares,
    DateTime? PlantedDate
) : BaseUpdateRequest(Id);