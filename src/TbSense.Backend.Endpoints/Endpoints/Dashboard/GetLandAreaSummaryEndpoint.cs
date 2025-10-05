using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetLandAreaSummaryEndpoint : BaseEndpointWithoutRequest<LandAreaSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/land-area");
        Group<DashboardEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        LandAreaSummaryResponse result = await DashboardService.GetLandAreaSummaryAsync(ct);
        await SendOkResponseAsync(result, "Land Area Summary Retrieved Successfully");
    }
}
