using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessions;

public class DeleteAiSessionEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public required IAiSessionService AiSessionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<AiSessionsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await AiSessionService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("AI Session Deleted Successfully", ct);
    }
}