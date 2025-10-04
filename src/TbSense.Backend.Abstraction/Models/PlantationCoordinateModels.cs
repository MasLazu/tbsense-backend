using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreatePlantationCoordinateRequest(
    Guid PlantationId,
    double Longitude,
    double Latitude
);

public record PlantationCoordinateDto(
    Guid Id,
    Guid PlantationId,
    double Longitude,
    double Latitude,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdatePlantationCoordinateRequest(
    Guid Id,
    Guid? PlantationId,
    double? Longitude,
    double? Latitude
) : BaseUpdateRequest(Id);