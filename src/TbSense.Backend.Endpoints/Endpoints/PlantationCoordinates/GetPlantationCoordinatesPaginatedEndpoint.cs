using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationCoordinates;

public class GetPlantationCoordinatesPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<PlantationCoordinateDto>>
{
    public IPlantationCoordinateService PlantationCoordinateService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<PlantationCoordinatesEndpointGroup>();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<PlantationCoordinateDto> result = await PlantationCoordinateService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Plantation Coordinates Paginated Retrieved Successfully", ct);
    }
}