using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationCoordinates;

public class DeletePlantationCoordinateEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public IPlantationCoordinateService PlantationCoordinateService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<PlantationCoordinatesEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await PlantationCoordinateService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Plantation Coordinate Deleted Successfully", ct);
    }
}