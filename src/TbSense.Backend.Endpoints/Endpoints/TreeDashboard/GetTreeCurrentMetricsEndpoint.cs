using FastEndpoints;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetTreeCurrentMetricsEndpoint : BaseEndpoint<TreeIdRequest, TreeCurrentMetricsResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/current-metrics");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeIdRequest req, CancellationToken ct)
    {
        TreeCurrentMetricsResponse? result = await TreeDashboardService.GetTreeCurrentMetricsAsync(req.TreeId, ct)
            ?? throw new NotFoundException(nameof(TreeCurrentMetricsResponse), req.TreeId);

        await SendOkResponseAsync(result, "Tree Current Metrics Retrieved Successfully", ct);
    }
}
