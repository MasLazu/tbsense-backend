using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetPlantationsSummaryEndpoint : BaseEndpoint<DashboardDateFilterRequest, PlantationsSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/plantations");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardDateFilterRequest req, CancellationToken ct)
    {
        PlantationsSummaryResponse result = await DashboardService.GetPlantationsSummaryAsync(req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Plantations Summary Retrieved Successfully", ct);
    }
}
