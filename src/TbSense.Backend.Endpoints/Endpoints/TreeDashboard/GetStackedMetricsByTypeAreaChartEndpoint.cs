using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetStackedMetricsByTypeAreaChartEndpoint : BaseEndpoint<TreeAreaChartRequest, TreeStackedMetricsByTypeResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/stacked-metrics-by-type");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        TreeStackedMetricsByTypeResponse result = await TreeDashboardService.GetStackedMetricsByTypeAreaChartAsync(
            req.TreeId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Stacked Metrics By Type Area Chart Retrieved Successfully", ct);
    }
}
