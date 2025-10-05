using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetSoilMoistureDistributionHistogramEndpoint : BaseEndpoint<PlantationHistogramDaysRequest, SoilMoistureDistributionHistogramResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{plantationId}/histogram/soil-moisture-distribution");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationHistogramDaysRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;
        int binCount = req.BinCount ?? 15;

        SoilMoistureDistributionHistogramResponse result = await PlantationDashboardService.GetSoilMoistureDistributionHistogramAsync(
            req.PlantationId,
            days,
            binCount,
            ct);

        await SendOkResponseAsync(result, "Soil Moisture Distribution Histogram Retrieved Successfully", ct);
    }
}
