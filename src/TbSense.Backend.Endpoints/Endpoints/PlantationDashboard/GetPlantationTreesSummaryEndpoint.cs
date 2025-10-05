using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationTreesSummaryEndpoint : BaseEndpoint<PlantationIdRequest, PlantationTreesSummaryResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/trees");
        Group<PlantationDashboardEndpointGroup>();
    }

    public override async Task HandleAsync(PlantationIdRequest req, CancellationToken ct)
    {
        PlantationTreesSummaryResponse result = await PlantationDashboardService.GetPlantationTreesSummaryAsync(req.PlantationId, ct);
        await SendOkResponseAsync(result, "Plantation Trees Summary Retrieved Successfully", ct);
    }
}
