using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetHarvestSummaryEndpoint : BaseEndpoint<DashboardDateFilterRequest, HarvestSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/harvest");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardDateFilterRequest req, CancellationToken ct)
    {
        HarvestSummaryResponse result = await DashboardService.GetHarvestSummaryAsync(req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Harvest Summary Retrieved Successfully", ct);
    }
}
