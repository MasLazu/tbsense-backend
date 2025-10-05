using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetWeeklyMetricsComparisonChartEndpoint : BaseEndpoint<GetWeeklyMetricsComparisonChartRequest, WeeklyMetricComparisonResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/bar-chart/weekly-metrics-comparison");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWeeklyMetricsComparisonChartRequest req, CancellationToken ct)
    {
        int weeks = req.Weeks ?? 8;

        WeeklyMetricComparisonResponse result = await TreeDashboardService.GetWeeklyMetricsComparisonChartAsync(
            req.TreeId,
            weeks,
            ct);

        await SendOkResponseAsync(result, "Weekly Metrics Comparison Chart Retrieved Successfully", ct);
    }
}
