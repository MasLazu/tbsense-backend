using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Plantations;

public class GetPlantationsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<PlantationDto>>
{
    public IPlantationService PlantationService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<PlantationsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<PlantationDto> result = await PlantationService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Plantations Paginated Retrieved Successfully", ct);
    }
}