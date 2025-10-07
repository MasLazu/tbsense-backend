using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateKnowledgeBaseRequest(
    string Title,
    string Content
);

public record KnowledgeBaseDto(
    Guid Id,
    string Title,
    string Content,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateKnowledgeBaseRequest(
    Guid Id,
    string? Title,
    string? Content
) : BaseUpdateRequest(Id);