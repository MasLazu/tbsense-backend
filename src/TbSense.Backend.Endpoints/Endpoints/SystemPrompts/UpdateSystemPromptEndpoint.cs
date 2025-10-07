using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.SystemPrompts;

public class UpdateSystemPromptEndpoint : BaseEndpoint<UpdateSystemPromptRequest, SystemPromptDto>
{
    public required ISystemPromptService SystemPromptService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<SystemPromptsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateSystemPromptRequest req, CancellationToken ct)
    {
        SystemPromptDto result = await SystemPromptService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "System Prompt Updated Successfully", ct);
    }
}