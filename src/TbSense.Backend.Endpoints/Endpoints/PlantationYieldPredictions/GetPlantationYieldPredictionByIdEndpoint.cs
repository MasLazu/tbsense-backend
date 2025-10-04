using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationYieldPredictions;

public class GetPlantationYieldPredictionByIdEndpoint : BaseEndpoint<IdRequest, PlantationYieldPredictionDto>
{
    public IPlantationYieldPredictionService PlantationYieldPredictionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<PlantationYieldPredictionsEndpointGroup>();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        PlantationYieldPredictionDto? result = await PlantationYieldPredictionService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(PlantationYieldPredictionDto), req.Id);
        await SendOkResponseAsync(result, "Plantation Yield Prediction Retrieved Successfully", ct);
    }
}