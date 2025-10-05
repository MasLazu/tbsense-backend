using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTreeActivityByPlantationChartEndpoint : BaseEndpoint<EmptyRequest, PlantationActivityComparisonResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/bar-chart/tree-activity-by-plantation");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        PlantationActivityComparisonResponse result = await DashboardService.GetTreeActivityByPlantationChartAsync(ct);
        await SendOkResponseAsync(result, "Tree Activity by Plantation Chart Retrieved Successfully", ct);
    }
}
