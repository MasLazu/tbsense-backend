using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationYieldPredictions;

public class DeletePlantationYieldPredictionEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public IPlantationYieldPredictionService PlantationYieldPredictionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<PlantationYieldPredictionsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await PlantationYieldPredictionService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Plantation Yield Prediction Deleted Successfully", ct);
    }
}