using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateAiSessionChatRequest(
    Guid SessionId,
    string Role,
    int Order,
    string Content
);

public record AiSessionChatDto(
    Guid Id,
    Guid SessionId,
    string Role,
    int Order,
    string Content,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateAiSessionChatRequest(
    Guid Id,
    Guid? SessionId,
    string? Role,
    int? Order,
    string? Content
) : BaseUpdateRequest(Id);