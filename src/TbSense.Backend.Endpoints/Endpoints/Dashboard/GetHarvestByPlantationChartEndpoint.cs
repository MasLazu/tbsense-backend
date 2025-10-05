using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetHarvestByPlantationChartEndpoint : BaseEndpoint<DashboardDateFilterRequest, HarvestDistributionResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/chart/harvest-by-plantation");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardDateFilterRequest req, CancellationToken ct)
    {
        HarvestDistributionResponse result = await DashboardService.GetHarvestByPlantationChartAsync(req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Harvest by Plantation Chart Retrieved Successfully", ct);
    }
}
