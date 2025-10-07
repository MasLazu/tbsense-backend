using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.SystemPrompts;

public class CreateSystemPromptEndpoint : BaseEndpoint<CreateSystemPromptRequest, SystemPromptDto>
{
    public required ISystemPromptService SystemPromptService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<SystemPromptsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateSystemPromptRequest req, CancellationToken ct)
    {
        SystemPromptDto result = await SystemPromptService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "System Prompt Created Successfully", ct);
    }
}