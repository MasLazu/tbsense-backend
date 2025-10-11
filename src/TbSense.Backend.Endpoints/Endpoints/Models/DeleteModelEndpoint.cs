using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Models;

public class DeleteModelEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public IModelService ModelService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<ModelsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await ModelService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Model Deleted Successfully", ct);
    }
}