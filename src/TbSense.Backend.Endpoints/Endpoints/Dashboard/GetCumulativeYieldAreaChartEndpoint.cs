using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetCumulativeYieldAreaChartEndpoint : BaseEndpoint<AreaChartIntervalRequest, CumulativeYieldResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-yield");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(AreaChartIntervalRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        CumulativeYieldResponse result = await DashboardService.GetCumulativeYieldAreaChartAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Cumulative Yield Area Chart Retrieved Successfully", ct);
    }
}
