using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Trees;

public class DeleteTreeEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public ITreeService TreeService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<TreesEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await TreeService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Tree Deleted Successfully", ct);
    }
}