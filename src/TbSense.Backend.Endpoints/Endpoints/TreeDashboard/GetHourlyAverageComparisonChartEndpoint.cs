using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetHourlyAverageComparisonChartEndpoint : BaseEndpoint<TreeDashboardQueryRequest, HourlyAverageComparisonResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/bar-chart/hourly-average-comparison");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeDashboardQueryRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;

        HourlyAverageComparisonResponse result = await TreeDashboardService.GetHourlyAverageComparisonChartAsync(
            req.TreeId,
            days,
            ct);

        await SendOkResponseAsync(result, "Hourly Average Comparison Chart Retrieved Successfully", ct);
    }
}
