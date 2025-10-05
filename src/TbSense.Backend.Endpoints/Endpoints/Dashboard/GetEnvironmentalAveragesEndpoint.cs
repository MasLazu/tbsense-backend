using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetEnvironmentalAveragesEndpoint : BaseEndpointWithoutRequest<EnvironmentalAveragesResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/environmental/averages");
        Group<DashboardEndpointGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        EnvironmentalAveragesResponse result = await DashboardService.GetEnvironmentalAveragesAsync(ct);
        await SendOkResponseAsync(result, "Environmental Averages Retrieved Successfully");
    }
}
