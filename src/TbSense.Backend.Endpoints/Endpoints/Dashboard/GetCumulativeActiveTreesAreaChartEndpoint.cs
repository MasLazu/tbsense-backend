using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetCumulativeActiveTreesAreaChartEndpoint : BaseEndpoint<AreaChartIntervalRequest, CumulativeActiveTreesResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-active-trees");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(AreaChartIntervalRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        CumulativeActiveTreesResponse result = await DashboardService.GetCumulativeActiveTreesAreaChartAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Cumulative Active Trees Area Chart Retrieved Successfully", ct);
    }
}
