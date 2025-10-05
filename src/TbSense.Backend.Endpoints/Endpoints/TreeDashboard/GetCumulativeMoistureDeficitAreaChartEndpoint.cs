using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetCumulativeMoistureDeficitAreaChartEndpoint : BaseEndpoint<TreeThresholdAreaChartRequest, CumulativeMoistureDeficitResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-moisture-deficit");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeThresholdAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";
        double threshold = req.Threshold ?? 30.0;

        CumulativeMoistureDeficitResponse result = await TreeDashboardService.GetCumulativeMoistureDeficitAreaChartAsync(
            req.TreeId,
            req.StartDate,
            req.EndDate,
            interval,
            threshold,
            ct);

        await SendOkResponseAsync(result, "Cumulative Moisture Deficit Area Chart Retrieved Successfully", ct);
    }
}
