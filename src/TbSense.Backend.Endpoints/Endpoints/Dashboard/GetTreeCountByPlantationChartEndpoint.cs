using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTreeCountByPlantationChartEndpoint : BaseEndpoint<GetTreeCountByPlantationChartRequest, PlantationTreeCountComparisonResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/bar-chart/tree-count-by-plantation");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTreeCountByPlantationChartRequest req, CancellationToken ct)
    {
        int limit = req.Limit ?? 10;
        PlantationTreeCountComparisonResponse result = await DashboardService.GetTreeCountByPlantationChartAsync(limit, ct);
        await SendOkResponseAsync(result, "Tree Count by Plantation Chart Retrieved Successfully", ct);
    }
}
