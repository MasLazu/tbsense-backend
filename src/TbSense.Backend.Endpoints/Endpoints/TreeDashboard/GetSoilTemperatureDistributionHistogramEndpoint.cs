using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetSoilTemperatureDistributionHistogramEndpoint : BaseEndpoint<TreeHistogramRequest, SoilTemperatureDistributionHistogramResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/histogram/soil-temperature-distribution");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeHistogramRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;
        int binCount = req.BinCount ?? 15;

        SoilTemperatureDistributionHistogramResponse result = await TreeDashboardService.GetSoilTemperatureDistributionHistogramAsync(
            req.TreeId,
            days,
            binCount,
            ct);

        await SendOkResponseAsync(result, "Soil Temperature Distribution Histogram Retrieved Successfully", ct);
    }
}
