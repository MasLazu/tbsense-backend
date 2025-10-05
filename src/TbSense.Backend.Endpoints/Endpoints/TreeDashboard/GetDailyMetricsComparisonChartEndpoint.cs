using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetDailyMetricsComparisonChartEndpoint : BaseEndpoint<TreeDashboardQueryRequest, DailyMetricComparisonResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/bar-chart/daily-metrics-comparison");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeDashboardQueryRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 7;

        DailyMetricComparisonResponse result = await TreeDashboardService.GetDailyMetricsComparisonChartAsync(
            req.TreeId,
            days,
            ct);

        await SendOkResponseAsync(result, "Daily Metrics Comparison Chart Retrieved Successfully", ct);
    }
}
