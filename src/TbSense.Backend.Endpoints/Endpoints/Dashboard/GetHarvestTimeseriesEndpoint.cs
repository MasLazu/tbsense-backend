using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetHarvestTimeseriesEndpoint : BaseEndpoint<DashboardTimeseriesRequest, HarvestTimeseriesResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/timeseries/harvest");
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

        HarvestTimeseriesResponse result = await DashboardService.GetHarvestTimeseriesAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Harvest Timeseries Retrieved Successfully", ct);
    }
}
