using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.TreeMetrics;

public class GetTreeMetricByIdEndpoint : BaseEndpoint<IdRequest, TreeMetricDto>
{
    public ITreeMetricService TreeMetricService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<TreeMetricsEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        TreeMetricDto? result = await TreeMetricService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(TreeMetricDto), req.Id);
        await SendOkResponseAsync(result, "Tree Metric Retrieved Successfully", ct);
    }
}