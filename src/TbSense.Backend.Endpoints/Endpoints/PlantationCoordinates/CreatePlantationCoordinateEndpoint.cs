using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationCoordinates;

public class CreatePlantationCoordinateEndpoint : BaseEndpoint<CreatePlantationCoordinateRequest, PlantationCoordinateDto>
{
    public IPlantationCoordinateService PlantationCoordinateService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<PlantationCoordinatesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreatePlantationCoordinateRequest req, CancellationToken ct)
    {
        PlantationCoordinateDto result = await PlantationCoordinateService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Coordinate Created Successfully", ct);
    }
}