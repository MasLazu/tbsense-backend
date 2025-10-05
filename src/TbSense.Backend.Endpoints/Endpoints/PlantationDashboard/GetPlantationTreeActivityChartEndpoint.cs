using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationTreeActivityChartEndpoint : BaseEndpoint<PlantationIdRequest, PlantationTreeActivityResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/chart/tree-activity");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationIdRequest req, CancellationToken ct)
    {
        PlantationTreeActivityResponse result = await PlantationDashboardService.GetPlantationTreeActivityChartAsync(req.PlantationId, ct);
        await SendOkResponseAsync(result, "Plantation Tree Activity Chart Retrieved Successfully", ct);
    }
}
