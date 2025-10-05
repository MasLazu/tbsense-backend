using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetCumulativeTemperatureExposureAreaChartEndpoint : BaseEndpoint<TreeThresholdAreaChartRequest, CumulativeTemperatureExposureResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-temperature-exposure");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeThresholdAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";
        double threshold = req.Threshold ?? 30.0;

        CumulativeTemperatureExposureResponse result = await TreeDashboardService.GetCumulativeTemperatureExposureAreaChartAsync(
            req.TreeId,
            req.StartDate,
            req.EndDate,
            interval,
            threshold,
            ct);

        await SendOkResponseAsync(result, "Cumulative Temperature Exposure Area Chart Retrieved Successfully", ct);
    }
}
