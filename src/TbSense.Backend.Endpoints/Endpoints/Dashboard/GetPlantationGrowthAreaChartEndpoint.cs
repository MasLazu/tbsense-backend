using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Dashboard;

public class GetPlantationGrowthAreaChartEndpoint : BaseEndpoint<AreaChartIntervalRequest, PlantationGrowthResponse>
{
    public IDashboardService DashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("area-chart/plantation-growth");
        Group<DashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(AreaChartIntervalRequest req, CancellationToken ct)
    {
        string interval = req.Interval ?? "daily";

        PlantationGrowthResponse result = await DashboardService.GetPlantationGrowthAreaChartAsync(
            req.StartDate,
            req.EndDate,
            interval,
            ct);

        await SendOkResponseAsync(result, "Plantation Growth Area Chart Retrieved Successfully", ct);
    }
}
