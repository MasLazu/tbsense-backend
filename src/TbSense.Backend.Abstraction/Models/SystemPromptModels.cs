using MasLazu.AspNet.Framework.Application.Models;

namespace TbSense.Backend.Abstraction.Models;

public record CreateSystemPromptRequest(
    string Prompt
);

public record SystemPromptDto(
    Guid Id,
    string Prompt,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);

public record UpdateSystemPromptRequest(
    Guid Id,
    string? Prompt
) : BaseUpdateRequest(Id);