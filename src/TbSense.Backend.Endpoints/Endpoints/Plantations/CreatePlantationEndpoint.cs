using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Plantations;

public class CreatePlantationEndpoint : BaseEndpoint<CreatePlantationRequest, PlantationDto>
{
    public IPlantationService PlantationService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<PlantationsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreatePlantationRequest req, CancellationToken ct)
    {
        PlantationDto result = await PlantationService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Created Successfully", ct);
    }
}