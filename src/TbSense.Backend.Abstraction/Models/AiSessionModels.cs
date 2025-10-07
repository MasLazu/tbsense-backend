using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateAiSessionRequest(
    string Title,
    DateTime LastActivityAt
);

public record AiSessionDto(
    Guid Id,
    string Title,
    DateTime LastActivityAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateAiSessionRequest(
    Guid Id,
    string? Title,
    DateTime? LastActivityAt
) : BaseUpdateRequest(Id);