using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetPlantationsByTreesChartEndpoint : BaseEndpoint<EmptyRequest, PlantationDistributionResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/chart/plantations-by-trees");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        PlantationDistributionResponse result = await DashboardService.GetPlantationsByTreesChartAsync(ct);
        await SendOkResponseAsync(result, "Plantations by Trees Chart Retrieved Successfully", ct);
    }
}
