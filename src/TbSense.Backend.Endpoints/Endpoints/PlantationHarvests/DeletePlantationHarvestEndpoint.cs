using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationHarvests;

public class DeletePlantationHarvestEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public IPlantationHarvestService PlantationHarvestService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<PlantationHarvestsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await PlantationHarvestService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Plantation Harvest Deleted Successfully", ct);
    }
}