using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Models;

public class TrainModelEndpoint : BaseEndpoint<TrainModelRequest, TrainModelResponse>
{
    public IModelService ModelService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/train");
        Group<ModelsEndpointGroup>();
    }

    public override async Task HandleAsync(TrainModelRequest req, CancellationToken ct)
    {
        TrainModelResponse result = await ModelService.TrainModelAsync(req, ct);
        await SendOkResponseAsync(result, "Model training initiated successfully", ct);
    }
}
