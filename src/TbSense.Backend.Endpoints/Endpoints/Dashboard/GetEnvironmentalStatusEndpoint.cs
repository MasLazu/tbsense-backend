using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetEnvironmentalStatusEndpoint : BaseEndpointWithoutRequest<EnvironmentalStatusResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/environmental/status");
        Group<DashboardEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        EnvironmentalStatusResponse result = await DashboardService.GetEnvironmentalStatusAsync(ct);
        await SendOkResponseAsync(result, "Environmental Status Retrieved Successfully");
    }
}
