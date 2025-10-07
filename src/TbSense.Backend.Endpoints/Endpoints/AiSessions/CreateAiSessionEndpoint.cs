using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessions;

public class CreateAiSessionEndpoint : BaseEndpoint<CreateAiSessionRequest, AiSessionDto>
{
    public required IAiSessionService AiSessionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<AiSessionsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateAiSessionRequest req, CancellationToken ct)
    {
        AiSessionDto result = await AiSessionService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "AI Session Created Successfully", ct);
    }
}