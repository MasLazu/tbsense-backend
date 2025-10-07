using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.SystemPrompts;

public class GetSystemPromptByIdEndpoint : BaseEndpoint<IdRequest, SystemPromptDto>
{
    public required ISystemPromptService SystemPromptService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<SystemPromptsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        SystemPromptDto? result = await SystemPromptService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(SystemPromptDto), req.Id);
        await SendOkResponseAsync(result, "System Prompt Retrieved Successfully", ct);
    }
}