using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetTreeAgeDistributionHistogramEndpoint : BaseEndpoint<PlantationHistogramBinCountRequest, TreeAgeDistributionHistogramResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{plantationId}/histogram/tree-age-distribution");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationHistogramBinCountRequest req, CancellationToken ct)
    {
        int binCount = req.BinCount ?? 10;

        TreeAgeDistributionHistogramResponse result = await PlantationDashboardService.GetTreeAgeDistributionHistogramAsync(
            req.PlantationId,
            binCount,
            ct);

        await SendOkResponseAsync(result, "Tree Age Distribution Histogram Retrieved Successfully", ct);
    }
}
