using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetTreeEnvironmentalTimeseriesEndpoint : BaseEndpoint<TreeTimeseriesRequest, TreeEnvironmentalTimeseriesResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/timeseries/environmental");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeTimeseriesRequest req, CancellationToken ct)
    {
        string interval = req.Interval?.ToLower() ?? "daily";

        // Validate interval
        if (!new[] { "hourly", "daily", "weekly", "monthly" }.Contains(interval))
        {
            interval = "daily";
        }

        TreeEnvironmentalTimeseriesResponse result = await TreeDashboardService.GetTreeEnvironmentalTimeseriesAsync(
            req.TreeId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Tree Environmental Timeseries Retrieved Successfully", ct);
    }
}
