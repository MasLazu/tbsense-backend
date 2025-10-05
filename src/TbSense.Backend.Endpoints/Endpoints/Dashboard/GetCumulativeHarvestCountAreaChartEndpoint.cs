using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetCumulativeHarvestCountAreaChartEndpoint : BaseEndpoint<AreaChartIntervalRequest, CumulativeHarvestCountResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-harvest-count");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(AreaChartIntervalRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        CumulativeHarvestCountResponse result = await DashboardService.GetCumulativeHarvestCountAreaChartAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Cumulative Harvest Count Area Chart Retrieved Successfully", ct);
    }
}
