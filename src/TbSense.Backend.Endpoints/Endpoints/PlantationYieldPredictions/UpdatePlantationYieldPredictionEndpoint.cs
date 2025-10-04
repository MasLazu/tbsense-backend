using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationYieldPredictions;

public class UpdatePlantationYieldPredictionEndpoint : BaseEndpoint<UpdatePlantationYieldPredictionRequest, PlantationYieldPredictionDto>
{
    public IPlantationYieldPredictionService PlantationYieldPredictionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<PlantationYieldPredictionsEndpointGroup>();
    }

    public override async Task HandleAsync(UpdatePlantationYieldPredictionRequest req, CancellationToken ct)
    {
        PlantationYieldPredictionDto result = await PlantationYieldPredictionService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Yield Prediction Updated Successfully", ct);
    }
}