using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetEnvironmentalAveragesEndpoint : BaseEndpoint<DashboardDateFilterRequest, EnvironmentalAveragesResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/environmental/averages");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardDateFilterRequest req, CancellationToken ct)
    {
        EnvironmentalAveragesResponse result = await DashboardService.GetEnvironmentalAveragesAsync(req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Environmental Averages Retrieved Successfully", ct);
    }
}
