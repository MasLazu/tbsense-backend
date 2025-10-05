using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetMonthlyHarvestComparisonChartEndpoint : BaseEndpoint<PlantationDateFilterRequest, MonthlyHarvestComparisonResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/bar-chart/monthly-harvest-comparison");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationDateFilterRequest req, CancellationToken ct)
    {
        MonthlyHarvestComparisonResponse result = await PlantationDashboardService.GetMonthlyHarvestComparisonChartAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            ct);

        await SendOkResponseAsync(result, "Monthly Harvest Comparison Chart Retrieved Successfully", ct);
    }
}
