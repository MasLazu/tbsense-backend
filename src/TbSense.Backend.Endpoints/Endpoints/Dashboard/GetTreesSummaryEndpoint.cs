using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTreesSummaryEndpoint : BaseEndpointWithoutRequest<TreesSummaryResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/summary/trees");
        Group<DashboardEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        TreesSummaryResponse result = await DashboardService.GetTreesSummaryAsync(ct);
        await SendOkResponseAsync(result, "Trees Summary Retrieved Successfully");
    }
}
