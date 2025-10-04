using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationHarvests;

public class UpdatePlantationHarvestEndpoint : BaseEndpoint<UpdatePlantationHarvestRequest, PlantationHarvestDto>
{
    public IPlantationHarvestService PlantationHarvestService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<PlantationHarvestsEndpointGroup>();
    }

    public override async Task HandleAsync(UpdatePlantationHarvestRequest req, CancellationToken ct)
    {
        PlantationHarvestDto result = await PlantationHarvestService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Harvest Updated Successfully", ct);
    }
}