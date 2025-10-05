using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationHarvestSummaryEndpoint : BaseEndpoint<PlantationDateFilterRequest, PlantationHarvestSummaryResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/harvest");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationDateFilterRequest req, CancellationToken ct)
    {
        PlantationHarvestSummaryResponse result = await PlantationDashboardService.GetPlantationHarvestSummaryAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            ct);
        await SendOkResponseAsync(result, "Plantation Harvest Summary Retrieved Successfully", ct);
    }
}
