using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Models;

public class GetModelsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<ModelDto>>
{
    public IModelService ModelService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<ModelsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<ModelDto> result = await ModelService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Models Paginated Retrieved Successfully", ct);
    }
}