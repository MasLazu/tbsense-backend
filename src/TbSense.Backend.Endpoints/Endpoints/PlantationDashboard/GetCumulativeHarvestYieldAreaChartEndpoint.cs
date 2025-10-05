using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetCumulativeHarvestYieldAreaChartEndpoint : BaseEndpoint<PlantationAreaChartRequest, PlantationCumulativeHarvestYieldResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-harvest-yield");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "weekly";

        PlantationCumulativeHarvestYieldResponse result = await PlantationDashboardService.GetCumulativeHarvestYieldAreaChartAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Cumulative Harvest Yield Area Chart Retrieved Successfully", ct);
    }
}
