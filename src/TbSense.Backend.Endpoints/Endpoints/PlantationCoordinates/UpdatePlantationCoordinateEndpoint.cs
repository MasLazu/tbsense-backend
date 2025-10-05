using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationCoordinates;

public class UpdatePlantationCoordinateEndpoint : BaseEndpoint<UpdatePlantationCoordinateRequest, PlantationCoordinateDto>
{
    public IPlantationCoordinateService PlantationCoordinateService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<PlantationCoordinatesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdatePlantationCoordinateRequest req, CancellationToken ct)
    {
        PlantationCoordinateDto result = await PlantationCoordinateService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Coordinate Updated Successfully", ct);
    }
}