using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetTreeActivityTrendChartEndpoint : BaseEndpoint<PlantationDateFilterRequest, MonthlyTreeActivityResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/bar-chart/tree-activity-trend");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationDateFilterRequest req, CancellationToken ct)
    {
        MonthlyTreeActivityResponse result = await PlantationDashboardService.GetTreeActivityTrendChartAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            ct);

        await SendOkResponseAsync(result, "Tree Activity Trend Chart Retrieved Successfully", ct);
    }
}
