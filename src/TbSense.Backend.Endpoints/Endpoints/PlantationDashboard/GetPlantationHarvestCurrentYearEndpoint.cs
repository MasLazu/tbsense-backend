using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationHarvestCurrentYearEndpoint : BaseEndpoint<PlantationIdRequest, PlantationHarvestSummaryResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/harvest-current-year");
        Group<PlantationDashboardEndpointGroup>();
    }

    public override async Task HandleAsync(PlantationIdRequest req, CancellationToken ct)
    {
        PlantationHarvestSummaryResponse result = await PlantationDashboardService.GetPlantationHarvestCurrentYearAsync(req.PlantationId, ct);
        await SendOkResponseAsync(result, "Plantation Current Year Harvest Retrieved Successfully", ct);
    }
}
