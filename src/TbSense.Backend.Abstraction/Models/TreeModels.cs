using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateTreeRequest(
    Guid PlantationId,
    double Longitude,
    double Latitude
);

public record TreeDto(
    Guid Id,
    Guid PlantationId,
    double Longitude,
    double Latitude,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateTreeRequest(
    Guid Id,
    Guid? PlantationId,
    double? Longitude,
    double? Latitude
) : BaseUpdateRequest(Id);