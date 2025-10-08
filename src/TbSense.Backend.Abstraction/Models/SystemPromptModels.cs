using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateSystemPromptRequest(
    string Name,
    string Prompt,
    bool? IsActive = true
);

public record SystemPromptDto(
    Guid Id,
    string Name,
    string Prompt,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateSystemPromptRequest(
    Guid Id,
    string? Name,
    string? Prompt,
    bool? IsActive
) : BaseUpdateRequest(Id);