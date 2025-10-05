using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetPlantationsSummaryEndpoint : BaseEndpointWithoutRequest<PlantationsSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/plantations");
        Group<DashboardEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        PlantationsSummaryResponse result = await DashboardService.GetPlantationsSummaryAsync(ct);
        await SendOkResponseAsync(result, "Plantations Summary Retrieved Successfully");
    }
}
