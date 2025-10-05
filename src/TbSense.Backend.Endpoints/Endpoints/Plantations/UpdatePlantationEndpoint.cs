using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Plantations;

public class UpdatePlantationEndpoint : BaseEndpoint<UpdatePlantationRequest, PlantationDto>
{
    public IPlantationService PlantationService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<PlantationsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdatePlantationRequest req, CancellationToken ct)
    {
        PlantationDto result = await PlantationService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Updated Successfully", ct);
    }
}