using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.TreeMetrics;

public class DeleteTreeMetricEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public ITreeMetricService TreeMetricService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<TreeMetricsEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await TreeMetricService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Tree Metric Deleted Successfully", ct);
    }
}