using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationHarvests;

public class GetPlantationHarvestByIdEndpoint : BaseEndpoint<IdRequest, PlantationHarvestDto>
{
    public IPlantationHarvestService PlantationHarvestService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<PlantationHarvestsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        PlantationHarvestDto? result = await PlantationHarvestService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(PlantationHarvestDto), req.Id);
        await SendOkResponseAsync(result, "Plantation Harvest Retrieved Successfully", ct);
    }
}