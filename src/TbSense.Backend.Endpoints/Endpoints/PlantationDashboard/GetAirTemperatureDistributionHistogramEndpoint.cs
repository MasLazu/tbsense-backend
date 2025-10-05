using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetAirTemperatureDistributionHistogramEndpoint : BaseEndpoint<PlantationHistogramDaysRequest, AirTemperatureDistributionHistogramResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{plantationId}/histogram/air-temperature-distribution");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationHistogramDaysRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;
        int binCount = req.BinCount ?? 15;

        AirTemperatureDistributionHistogramResponse result = await PlantationDashboardService.GetAirTemperatureDistributionHistogramAsync(
            req.PlantationId,
            days,
            binCount,
            ct);

        await SendOkResponseAsync(result, "Air Temperature Distribution Histogram Retrieved Successfully", ct);
    }
}
