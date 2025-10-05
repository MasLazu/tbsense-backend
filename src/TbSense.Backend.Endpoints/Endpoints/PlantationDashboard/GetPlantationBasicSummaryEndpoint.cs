using FastEndpoints;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.PlantationDashboard;

public class GetPlantationBasicSummaryEndpoint : BaseEndpoint<PlantationIdRequest, PlantationBasicSummaryResponse>
{
    public IPlantationDashboardService PlantationDashboardService { get; set; } = null!;

    public override void ConfigureEndpoint()
    {
        Get("/{plantationId}/summary/basic");
        Group<PlantationDashboardEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PlantationIdRequest req, CancellationToken ct)
    {
        PlantationBasicSummaryResponse? result = await PlantationDashboardService.GetPlantationBasicSummaryAsync(req.PlantationId, ct)
            ?? throw new NotFoundException(nameof(PlantationBasicSummaryResponse), req.PlantationId);
        await SendOkResponseAsync(result, "Plantation Basic Summary Retrieved Successfully", ct);
    }
}
