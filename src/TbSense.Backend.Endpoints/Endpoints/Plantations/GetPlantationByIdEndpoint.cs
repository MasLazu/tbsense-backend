using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.Plantations;

public class GetPlantationByIdEndpoint : BaseEndpoint<IdRequest, PlantationDto>
{
    public IPlantationService PlantationService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<PlantationsEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        PlantationDto? result = await PlantationService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(PlantationDto), req.Id);
        await SendOkResponseAsync(result, "Plantation Retrieved Successfully", ct);
    }
}