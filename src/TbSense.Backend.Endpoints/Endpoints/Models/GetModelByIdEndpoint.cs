using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.Models;

public class GetModelByIdEndpoint : BaseEndpoint<IdRequest, ModelDto>
{
    public IModelService ModelService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<ModelsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        ModelDto? result = await ModelService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(ModelDto), req.Id);
        await SendOkResponseAsync(result, "Model Retrieved Successfully", ct);
    }
}