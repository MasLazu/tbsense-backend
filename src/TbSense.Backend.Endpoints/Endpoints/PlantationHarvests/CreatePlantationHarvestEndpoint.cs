using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationHarvests;

public class CreatePlantationHarvestEndpoint : BaseEndpoint<CreatePlantationHarvestRequest, PlantationHarvestDto>
{
    public IPlantationHarvestService PlantationHarvestService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<PlantationHarvestsEndpointGroup>();
    }

    public override async Task HandleAsync(CreatePlantationHarvestRequest req, CancellationToken ct)
    {
        PlantationHarvestDto result = await PlantationHarvestService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Harvest Created Successfully", ct);
    }
}