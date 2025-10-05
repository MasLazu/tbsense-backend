using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetTopTreesByMetricChartEndpoint : BaseEndpoint<GetTopTreesByMetricChartRequest, TopTreeByMetricResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/bar-chart/top-trees-by-metric");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTopTreesByMetricChartRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;
        int limit = req.Limit ?? 10;
        string metricType = req.MetricType ?? "AirTemperature";

        TopTreeByMetricResponse result = await PlantationDashboardService.GetTopTreesByMetricChartAsync(
            req.PlantationId,
            metricType,
            days,
            limit,
            ct);

        await SendOkResponseAsync(result, "Top Trees by Metric Chart Retrieved Successfully", ct);
    }
}
