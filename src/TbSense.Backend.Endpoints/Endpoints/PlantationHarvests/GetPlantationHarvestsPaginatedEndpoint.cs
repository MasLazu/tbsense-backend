using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationHarvests;

public class GetPlantationHarvestsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<PlantationHarvestDto>>
{
    public IPlantationHarvestService PlantationHarvestService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<PlantationHarvestsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<PlantationHarvestDto> result = await PlantationHarvestService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Plantation Harvests Paginated Retrieved Successfully", ct);
    }
}