using FastEndpoints;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationRankingEndpoint : BaseEndpoint<PlantationDateFilterRequest, PlantationRankingResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/ranking");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationDateFilterRequest req, CancellationToken ct)
    {
        PlantationRankingResponse? result = await PlantationDashboardService.GetPlantationRankingAsync(
            req.PlantationId,
            req.StartDate,
            req.EndDate,
            ct) ?? throw new NotFoundException(nameof(PlantationRankingResponse), req.PlantationId);

        await SendOkResponseAsync(result, "Plantation Ranking Retrieved Successfully", ct);
    }
}
