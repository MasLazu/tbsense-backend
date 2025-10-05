using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTreesSummaryEndpoint : BaseEndpoint<DashboardDateFilterRequest, TreesSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/trees");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardDateFilterRequest req, CancellationToken ct)
    {
        TreesSummaryResponse result = await DashboardService.GetTreesSummaryAsync(req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Trees Summary Retrieved Successfully", ct);
    }
}
