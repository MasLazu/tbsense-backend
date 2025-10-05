using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Plantations;

public class DeletePlantationEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public IPlantationService PlantationService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<PlantationsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await PlantationService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Plantation Deleted Successfully", ct);
    }
}