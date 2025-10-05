using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetTreeActivityStatusChartEndpoint : BaseEndpoint<EmptyRequest, TreeActivityStatusResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/chart/tree-activity-status");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        TreeActivityStatusResponse result = await DashboardService.GetTreeActivityStatusChartAsync(ct);
        await SendOkResponseAsync(result, "Tree Activity Status Chart Retrieved Successfully", ct);
    }
}
