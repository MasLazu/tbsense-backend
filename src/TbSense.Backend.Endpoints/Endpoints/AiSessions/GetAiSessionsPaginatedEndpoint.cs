using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessions;

public class GetAiSessionsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<AiSessionDto>>
{
    public IAiSessionService AiSessionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<AiSessionsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<AiSessionDto> result = await AiSessionService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "AI Sessions Paginated Retrieved Successfully", ct);
    }
}