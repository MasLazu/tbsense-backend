using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetCumulativeMetricReadingsAreaChartEndpoint : BaseEndpoint<TreeAreaChartRequest, TreeCumulativeMetricReadingsResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-metric-readings");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        TreeCumulativeMetricReadingsResponse result = await TreeDashboardService.GetCumulativeMetricReadingsAreaChartAsync(
            req.TreeId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Cumulative Metric Readings Area Chart Retrieved Successfully", ct);
    }
}
