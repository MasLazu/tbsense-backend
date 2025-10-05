using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetTreeMetricRangesChartEndpoint : BaseEndpoint<TreeMetricRangesRequest, MetricRangesResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/chart/metric-ranges");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeMetricRangesRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;
        string metricType = req.MetricType ?? "AirTemperature";

        MetricRangesResponse result = await TreeDashboardService.GetTreeMetricRangesChartAsync(req.TreeId, metricType, days, ct);
        await SendOkResponseAsync(result, "Tree Metric Ranges Chart Retrieved Successfully", ct);
    }
}