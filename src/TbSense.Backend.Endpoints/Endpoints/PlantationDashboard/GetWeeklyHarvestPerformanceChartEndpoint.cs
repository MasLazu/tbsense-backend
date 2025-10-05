using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetWeeklyHarvestPerformanceChartEndpoint : BaseEndpoint<GetWeeklyHarvestPerformanceChartRequest, WeeklyHarvestPerformanceResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/bar-chart/weekly-harvest-performance");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetWeeklyHarvestPerformanceChartRequest req, CancellationToken ct)
    {
        int weeks = req.Weeks ?? 12;

        WeeklyHarvestPerformanceResponse result = await PlantationDashboardService.GetWeeklyHarvestPerformanceChartAsync(
            req.PlantationId,
            weeks,
            ct);

        await SendOkResponseAsync(result, "Weekly Harvest Performance Chart Retrieved Successfully", ct);
    }
}
