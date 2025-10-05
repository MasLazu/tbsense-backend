using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetTreeMonitoringAdoptionAreaChartEndpoint : BaseEndpoint<PlantationAreaChartRequest, TreeMonitoringAdoptionResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/tree-monitoring-adoption");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "weekly";

        TreeMonitoringAdoptionResponse result = await PlantationDashboardService.GetTreeMonitoringAdoptionAreaChartAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Tree Monitoring Adoption Area Chart Retrieved Successfully", ct);
    }
}
