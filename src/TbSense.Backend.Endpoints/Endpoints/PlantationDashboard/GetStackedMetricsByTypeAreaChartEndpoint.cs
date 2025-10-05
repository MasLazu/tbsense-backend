using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetStackedMetricsByTypeAreaChartEndpoint : BaseEndpoint<PlantationAreaChartRequest, StackedMetricsByTypeResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/stacked-metrics-by-type");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationAreaChartRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        StackedMetricsByTypeResponse result = await PlantationDashboardService.GetStackedMetricsByTypeAreaChartAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Stacked Metrics By Type Area Chart Retrieved Successfully", ct);
    }
}
