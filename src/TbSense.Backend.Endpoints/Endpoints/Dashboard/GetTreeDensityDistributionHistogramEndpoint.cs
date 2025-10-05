using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTreeDensityDistributionHistogramEndpoint : BaseEndpoint<HistogramBinCountRequest, TreeDensityDistributionHistogramResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("histogram/tree-density-distribution");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(HistogramBinCountRequest req, CancellationToken ct)
    {
        int binCount = req.BinCount ?? 10;

        TreeDensityDistributionHistogramResponse result = await DashboardService.GetTreeDensityDistributionHistogramAsync(
            binCount,
            ct);

        await SendOkResponseAsync(result, "Tree Density Distribution Histogram Retrieved Successfully", ct);
    }
}
