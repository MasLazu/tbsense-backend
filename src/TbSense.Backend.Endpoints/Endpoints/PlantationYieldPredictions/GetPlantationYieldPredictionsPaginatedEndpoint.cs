using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationYieldPredictions;

public class GetPlantationYieldPredictionsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<PlantationYieldPredictionDto>>
{
    public IPlantationYieldPredictionService PlantationYieldPredictionService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<PlantationYieldPredictionsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<PlantationYieldPredictionDto> result = await PlantationYieldPredictionService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Plantation Yield Predictions Paginated Retrieved Successfully", ct);
    }
}