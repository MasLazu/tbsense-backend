using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationHarvestTimeseriesEndpoint : BaseEndpoint<PlantationTimeseriesRequest, PlantationHarvestTimeseriesResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/timeseries/harvest");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationTimeseriesRequest req, CancellationToken ct)
    {
        string interval = req.Interval?.ToLower() ?? "monthly";

        if (!new[] { "daily", "weekly", "monthly", "yearly" }.Contains(interval))
        {
            interval = "monthly";
        }

        PlantationHarvestTimeseriesResponse result = await PlantationDashboardService.GetPlantationHarvestTimeseriesAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Plantation Harvest Timeseries Retrieved Successfully", ct);
    }
}
