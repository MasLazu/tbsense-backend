using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Trees;

public class CreateTreeEndpoint : BaseEndpoint<CreateTreeRequest, TreeDto>
{
    public ITreeService TreeService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<TreesEndpointGroup>();
    }

    public override async Task HandleAsync(CreateTreeRequest req, CancellationToken ct)
    {
        TreeDto result = await TreeService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Tree Created Successfully", ct);
    }
}