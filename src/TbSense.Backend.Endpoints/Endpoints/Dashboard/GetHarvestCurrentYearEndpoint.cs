using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetHarvestCurrentYearEndpoint : BaseEndpointWithoutRequest<HarvestSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/harvest-current-year");
        Group<DashboardEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        HarvestSummaryResponse result = await DashboardService.GetHarvestCurrentYearAsync(ct);
        await SendOkResponseAsync(result, "Current Year Harvest Summary Retrieved Successfully");
    }
}
