using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Trees;

public class UpdateTreeEndpoint : BaseEndpoint<UpdateTreeRequest, TreeDto>
{
    public ITreeService TreeService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<TreesEndpointGroup>();
    }

    public override async Task HandleAsync(UpdateTreeRequest req, CancellationToken ct)
    {
        TreeDto result = await TreeService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Tree Updated Successfully", ct);
    }
}