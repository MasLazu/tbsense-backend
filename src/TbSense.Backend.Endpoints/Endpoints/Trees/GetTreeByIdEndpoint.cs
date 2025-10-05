using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.Trees;

public class GetTreeByIdEndpoint : BaseEndpoint<IdRequest, TreeDto>
{
    public ITreeService TreeService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<TreesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        TreeDto? result = await TreeService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(TreeDto), req.Id);
        await SendOkResponseAsync(result, "Tree Retrieved Successfully", ct);
    }
}