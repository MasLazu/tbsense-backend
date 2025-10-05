using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetLandAreaSummaryEndpoint : BaseEndpoint<DashboardDateFilterRequest, LandAreaSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/land-area");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardDateFilterRequest req, CancellationToken ct)
    {
        LandAreaSummaryResponse result = await DashboardService.GetLandAreaSummaryAsync(req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Land Area Summary Retrieved Successfully", ct);
    }
}
