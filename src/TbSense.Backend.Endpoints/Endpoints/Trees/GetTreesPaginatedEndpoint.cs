using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Trees;

public class GetTreesPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<TreeDto>>
{
    public ITreeService TreeService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<TreesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<TreeDto> result = await TreeService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Trees Paginated Retrieved Successfully", ct);
    }
}