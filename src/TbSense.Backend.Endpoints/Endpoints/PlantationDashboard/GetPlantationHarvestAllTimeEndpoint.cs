using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationHarvestAllTimeEndpoint : BaseEndpoint<PlantationIdRequest, PlantationHarvestAllTimeResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/harvest-all-time");
        Group<PlantationDashboardEndpointGroup>();
    }

    public override async Task HandleAsync(PlantationIdRequest req, CancellationToken ct)
    {
        PlantationHarvestAllTimeResponse result = await PlantationDashboardService.GetPlantationHarvestAllTimeAsync(req.PlantationId, ct);
        await SendOkResponseAsync(result, "Plantation All-Time Harvest Retrieved Successfully", ct);
    }
}
