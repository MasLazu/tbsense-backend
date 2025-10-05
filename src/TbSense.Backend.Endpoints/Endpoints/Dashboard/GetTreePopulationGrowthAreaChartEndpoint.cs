using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTreePopulationGrowthAreaChartEndpoint : BaseEndpoint<AreaChartIntervalRequest, TreePopulationGrowthResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/tree-population-growth");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(AreaChartIntervalRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        TreePopulationGrowthResponse result = await DashboardService.GetTreePopulationGrowthAreaChartAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Tree Population Growth Area Chart Retrieved Successfully", ct);
    }
}
