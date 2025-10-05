using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetHarvestFrequencyByPlantationChartEndpoint : BaseEndpoint<DashboardDateFilterRequest, PlantationHarvestFrequencyResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/bar-chart/harvest-frequency-by-plantation");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardDateFilterRequest req, CancellationToken ct)
    {
        PlantationHarvestFrequencyResponse result = await DashboardService.GetHarvestFrequencyByPlantationChartAsync(req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Harvest Frequency by Plantation Chart Retrieved Successfully", ct);
    }
}
