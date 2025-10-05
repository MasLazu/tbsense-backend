using FastEndpoints;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.TreeDashboard;

public class GetTreeBasicInfoEndpoint : BaseEndpoint<TreeIdRequest, TreeBasicInfoResponse>
{
    public ITreeDashboardService TreeDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("{treeId}/basic");
        Group<TreeDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(TreeIdRequest req, CancellationToken ct)
    {
        TreeBasicInfoResponse? result = await TreeDashboardService.GetTreeBasicInfoAsync(req.TreeId, ct)
            ?? throw new NotFoundException(nameof(TreeBasicInfoResponse), req.TreeId);

        await SendOkResponseAsync(result, "Tree Basic Information Retrieved Successfully", ct);
    }
}
