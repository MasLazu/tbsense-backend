using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationEnvironmentalZonesChartEndpoint : BaseEndpoint<PlantationDashboardQueryRequest, EnvironmentalZonesResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/chart/environmental-zones");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationDashboardQueryRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;
        EnvironmentalZonesResponse result = await PlantationDashboardService.GetPlantationEnvironmentalZonesChartAsync(req.PlantationId, days, ct);
        await SendOkResponseAsync(result, "Plantation Environmental Zones Chart Retrieved Successfully", ct);
    }
}
