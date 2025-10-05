using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetTreesByZoneChartEndpoint : BaseEndpoint<PlantationDashboardQueryRequest, TreesByZoneComparisonResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/bar-chart/trees-by-zone");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationDashboardQueryRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;

        TreesByZoneComparisonResponse result = await PlantationDashboardService.GetTreesByZoneChartAsync(
            req.PlantationId,
            days,
            ct);

        await SendOkResponseAsync(result, "Trees by Zone Chart Retrieved Successfully", ct);
    }
}
