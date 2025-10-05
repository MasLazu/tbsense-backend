using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTopPlantationsByAvgHarvestChartEndpoint : BaseEndpoint<GetTopPlantationsByAvgHarvestChartRequest, PlantationAvgHarvestComparisonResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/bar-chart/top-plantations-by-avg-harvest");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTopPlantationsByAvgHarvestChartRequest req, CancellationToken ct)
    {
        int limit = req.Limit ?? 10;
        PlantationAvgHarvestComparisonResponse result = await DashboardService.GetTopPlantationsByAvgHarvestChartAsync(req.StartDate, req.EndDate, limit, ct);
        await SendOkResponseAsync(result, "Top Plantations by Average Harvest Chart Retrieved Successfully", ct);
    }
}
