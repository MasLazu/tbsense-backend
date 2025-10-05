using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTopPlantationsByYieldChartEndpoint : BaseEndpoint<GetTopPlantationsByYieldChartRequest, PlantationYieldComparisonResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/bar-chart/top-plantations-by-yield");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTopPlantationsByYieldChartRequest req, CancellationToken ct)
    {
        int limit = req.Limit ?? 10;
        PlantationYieldComparisonResponse result = await DashboardService.GetTopPlantationsByYieldChartAsync(req.StartDate, req.EndDate, limit, ct);
        await SendOkResponseAsync(result, "Top Plantations by Yield Chart Retrieved Successfully", ct);
    }
}
