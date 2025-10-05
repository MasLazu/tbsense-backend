using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetMetricRangeComparisonChartEndpoint : BaseEndpoint<TreeDashboardQueryRequest, MetricRangeComparisonResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/bar-chart/metric-range-comparison");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeDashboardQueryRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;

        MetricRangeComparisonResponse result = await TreeDashboardService.GetMetricRangeComparisonChartAsync(
            req.TreeId,
            days,
            ct);

        await SendOkResponseAsync(result, "Metric Range Comparison Chart Retrieved Successfully", ct);
    }
}
