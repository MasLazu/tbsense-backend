using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationHarvestByMonthChartEndpoint : BaseEndpoint<PlantationDateFilterRequest, MonthlyHarvestDistributionResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/chart/harvest-by-month");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationDateFilterRequest req, CancellationToken ct)
    {
        MonthlyHarvestDistributionResponse result = await PlantationDashboardService.GetPlantationHarvestByMonthChartAsync(req.PlantationId, req.StartDate, req.EndDate, ct);
        await SendOkResponseAsync(result, "Plantation Monthly Harvest Chart Retrieved Successfully", ct);
    }
}
