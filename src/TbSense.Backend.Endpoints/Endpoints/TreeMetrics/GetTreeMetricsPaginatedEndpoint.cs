using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.TreeMetrics;

public class GetTreeMetricsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<TreeMetricDto>>
{
    public ITreeMetricService TreeMetricService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<TreeMetricsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<TreeMetricDto> result = await TreeMetricService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Tree Metrics Paginated Retrieved Successfully", ct);
    }
}