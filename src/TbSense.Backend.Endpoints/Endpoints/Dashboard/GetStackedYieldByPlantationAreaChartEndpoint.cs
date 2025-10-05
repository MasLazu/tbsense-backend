using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetStackedYieldByPlantationAreaChartEndpoint : BaseEndpoint<StackedAreaChartRequest, StackedYieldByPlantationResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/stacked-yield-by-plantation");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(StackedAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "monthly";
        int limit = req.Limit ?? 5;

        StackedYieldByPlantationResponse result = await DashboardService.GetStackedYieldByPlantationAreaChartAsync(
            req.StartDate,
            req.EndDate,
            interval,
            limit,
            ct);

        await SendOkResponseAsync(result, "Stacked Yield By Plantation Area Chart Retrieved Successfully", ct);
    }
}
