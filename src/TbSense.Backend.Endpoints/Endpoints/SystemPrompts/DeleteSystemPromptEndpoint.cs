using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.SystemPrompts;

public class DeleteSystemPromptEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public required ISystemPromptService SystemPromptService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<SystemPromptsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await SystemPromptService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("System Prompt Deleted Successfully", ct);
    }
}