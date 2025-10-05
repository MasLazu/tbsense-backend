using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetEnvironmentalTimeseriesEndpoint : BaseEndpoint<DashboardTimeseriesRequest, EnvironmentalTimeseriesResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/timeseries/environmental");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(DashboardTimeseriesRequest req, CancellationToken ct)
    {
        string interval = req.Interval?.ToLower() ?? "daily";

        if (!new[] { "hourly", "daily", "weekly", "monthly" }.Contains(interval))
        {
            interval = "daily";
        }

        EnvironmentalTimeseriesResponse result = await DashboardService.GetEnvironmentalTimeseriesAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Environmental Timeseries Retrieved Successfully", ct);
    }
}
