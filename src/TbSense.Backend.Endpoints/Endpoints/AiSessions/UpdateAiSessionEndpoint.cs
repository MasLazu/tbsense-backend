using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessions;

public class UpdateAiSessionEndpoint : BaseEndpoint<UpdateAiSessionRequest, AiSessionDto>
{
    public required IAiSessionService AiSessionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<AiSessionsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateAiSessionRequest req, CancellationToken ct)
    {
        AiSessionDto result = await AiSessionService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "AI Session Updated Successfully", ct);
    }
}