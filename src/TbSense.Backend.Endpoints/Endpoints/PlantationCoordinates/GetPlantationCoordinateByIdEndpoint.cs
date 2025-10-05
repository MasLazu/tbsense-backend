using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationCoordinates;

public class GetPlantationCoordinateByIdEndpoint : BaseEndpoint<IdRequest, PlantationCoordinateDto>
{
    public IPlantationCoordinateService PlantationCoordinateService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<PlantationCoordinatesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        PlantationCoordinateDto? result = await PlantationCoordinateService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(PlantationCoordinateDto), req.Id);
        await SendOkResponseAsync(result, "Plantation Coordinate Retrieved Successfully", ct);
    }
}