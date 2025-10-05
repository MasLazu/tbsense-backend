using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetCumulativeMetricReadingsAreaChartEndpoint : BaseEndpoint<PlantationAreaChartRequest, CumulativeMetricReadingsResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/cumulative-metric-readings");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        CumulativeMetricReadingsResponse result = await PlantationDashboardService.GetCumulativeMetricReadingsAreaChartAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Cumulative Metric Readings Area Chart Retrieved Successfully", ct);
    }
}
