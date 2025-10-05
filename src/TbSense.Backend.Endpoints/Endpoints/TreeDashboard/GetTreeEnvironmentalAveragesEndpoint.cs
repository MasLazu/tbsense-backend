using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetTreeEnvironmentalAveragesEndpoint : BaseEndpoint<TreeDashboardQueryRequest, TreeEnvironmentalAveragesResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/environmental-averages");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeDashboardQueryRequest req, CancellationToken ct)
    {
        int days = req.Days ?? 30;

        TreeEnvironmentalAveragesResponse result = await TreeDashboardService.GetTreeEnvironmentalAveragesAsync(
            req.TreeId,
            days,
            ct);

        await SendOkResponseAsync(result, "Tree Environmental Averages Retrieved Successfully", ct);
    }
}
