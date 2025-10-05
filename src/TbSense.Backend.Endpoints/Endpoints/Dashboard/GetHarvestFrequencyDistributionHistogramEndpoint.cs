using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetHarvestFrequencyDistributionHistogramEndpoint : BaseEndpoint<HistogramDateFilterRequest, HarvestFrequencyDistributionHistogramResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("histogram/harvest-frequency-distribution");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(HistogramDateFilterRequest req, CancellationToken ct)
    {
        int binCount = req.BinCount ?? 10;

        HarvestFrequencyDistributionHistogramResponse result = await DashboardService.GetHarvestFrequencyDistributionHistogramAsync(
            req.StartDate,
            req.EndDate,
            binCount,
            ct);

        await SendOkResponseAsync(result, "Harvest Frequency Distribution Histogram Retrieved Successfully", ct);
    }
}
