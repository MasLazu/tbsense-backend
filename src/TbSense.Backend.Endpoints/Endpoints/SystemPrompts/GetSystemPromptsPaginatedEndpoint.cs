using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.SystemPrompts;

public class GetSystemPromptsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<SystemPromptDto>>
{
    public ISystemPromptService SystemPromptService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<SystemPromptsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<SystemPromptDto> result = await SystemPromptService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "System Prompts Paginated Retrieved Successfully", ct);
    }
}