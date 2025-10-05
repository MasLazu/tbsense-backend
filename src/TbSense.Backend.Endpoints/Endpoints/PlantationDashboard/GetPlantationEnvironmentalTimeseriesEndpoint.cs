using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationEnvironmentalTimeseriesEndpoint : BaseEndpoint<PlantationTimeseriesRequest, PlantationEnvironmentalTimeseriesResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/timeseries/environmental");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationTimeseriesRequest req, CancellationToken ct)
    {
        string interval = req.Interval?.ToLower() ?? "daily";

        // Validate interval
        if (!new[] { "hourly", "daily", "weekly", "monthly" }.Contains(interval))
        {
            interval = "daily";
        }

        PlantationEnvironmentalTimeseriesResponse result = await PlantationDashboardService.GetPlantationEnvironmentalTimeseriesAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Plantation Environmental Timeseries Retrieved Successfully", ct);
    }
}
