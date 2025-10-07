using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessions;

public class GetAiSessionByIdEndpoint : BaseEndpoint<IdRequest, AiSessionDto>
{
    public required IAiSessionService AiSessionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<AiSessionsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        AiSessionDto? result = await AiSessionService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(AiSessionDto), req.Id);
        await SendOkResponseAsync(result, "AI Session Retrieved Successfully", ct);
    }
}