using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetPlantationSizeDistributionHistogramEndpoint : BaseEndpoint<HistogramBinCountRequest, PlantationSizeDistributionHistogramResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("histogram/plantation-size-distribution");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(HistogramBinCountRequest req, CancellationToken ct)
    {
        int binCount = req.BinCount ?? 10;

        PlantationSizeDistributionHistogramResponse result = await DashboardService.GetPlantationSizeDistributionHistogramAsync(
            binCount,
            ct);

        await SendOkResponseAsync(result, "Plantation Size Distribution Histogram Retrieved Successfully", ct);
    }
}
