using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateTreeRequest(
    Guid PlantationId
);

public record TreeDto(
    Guid Id,
    Guid PlantationId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateTreeRequest(
    Guid Id,
    Guid? PlantationId
) : BaseUpdateRequest(Id);