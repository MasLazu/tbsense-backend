using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetPlantationsByLandAreaChartEndpoint : BaseEndpoint<EmptyRequest, PlantationLandDistributionResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/chart/plantations-by-land-area");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        PlantationLandDistributionResponse result = await DashboardService.GetPlantationsByLandAreaChartAsync(ct);
        await SendOkResponseAsync(result, "Plantations by Land Area Chart Retrieved Successfully", ct);
    }
}
