using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetTreeYieldDistributionHistogramEndpoint : BaseEndpoint<PlantationHistogramDateFilterRequest, TreeYieldDistributionHistogramResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{plantationId}/histogram/tree-yield-distribution");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationHistogramDateFilterRequest req, CancellationToken ct)
    {
        int binCount = req.BinCount ?? 10;

        TreeYieldDistributionHistogramResponse result = await PlantationDashboardService.GetTreeYieldDistributionHistogramAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            binCount,
            ct);

        await SendOkResponseAsync(result, "Tree Yield Distribution Histogram Retrieved Successfully", ct);
    }
}
