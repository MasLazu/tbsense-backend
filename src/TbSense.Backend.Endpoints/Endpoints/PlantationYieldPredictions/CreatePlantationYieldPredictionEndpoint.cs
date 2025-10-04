using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationYieldPredictions;

public class CreatePlantationYieldPredictionEndpoint : BaseEndpoint<CreatePlantationYieldPredictionRequest, PlantationYieldPredictionDto>
{
    public IPlantationYieldPredictionService PlantationYieldPredictionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<PlantationYieldPredictionsEndpointGroup>();
    }

    public override async Task HandleAsync(CreatePlantationYieldPredictionRequest req, CancellationToken ct)
    {
        PlantationYieldPredictionDto result = await PlantationYieldPredictionService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Plantation Yield Prediction Created Successfully", ct);
    }
}