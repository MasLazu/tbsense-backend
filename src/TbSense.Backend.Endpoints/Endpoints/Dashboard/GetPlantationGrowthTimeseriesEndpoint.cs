using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetPlantationGrowthTimeseriesEndpoint : BaseEndpoint<DashboardTimeseriesRequest, PlantationGrowthTimeseriesResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/timeseries/plantations");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardTimeseriesRequest req, CancellationToken ct)
    {
        string interval = req.Interval?.ToLower() ?? "monthly";

        if (!new[] { "daily", "weekly", "monthly", "yearly" }.Contains(interval))
        {
            interval = "monthly";
        }

        PlantationGrowthTimeseriesResponse result = await DashboardService.GetPlantationGrowthTimeseriesAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Plantation Growth Timeseries Retrieved Successfully", ct);
    }
}
