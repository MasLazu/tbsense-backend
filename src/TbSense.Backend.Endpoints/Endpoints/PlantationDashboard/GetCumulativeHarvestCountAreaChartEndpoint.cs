using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetCumulativeHarvestCountAreaChartEndpoint : BaseEndpoint<PlantationAreaChartRequest, PlantationCumulativeHarvestCountResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-harvest-count");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "weekly";

        PlantationCumulativeHarvestCountResponse result = await PlantationDashboardService.GetCumulativeHarvestCountAreaChartAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Cumulative Harvest Count Area Chart Retrieved Successfully", ct);
    }
}
