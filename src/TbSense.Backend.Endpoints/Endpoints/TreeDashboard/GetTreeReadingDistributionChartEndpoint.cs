using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetTreeReadingDistributionChartEndpoint : BaseEndpoint<TreeDashboardQueryRequest, ReadingDistributionResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/chart/reading-distribution");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeDashboardQueryRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-days);

        ReadingDistributionResponse result = await TreeDashboardService.GetTreeReadingDistributionChartAsync(req.TreeId, startDate, endDate, ct);
        await SendOkResponseAsync(result, "Tree Reading Distribution Chart Retrieved Successfully", ct);
    }
}
