using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Models;

public class CreateModelEndpoint : BaseEndpoint<CreateModelRequest, ModelDto>
{
    public IModelService ModelService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<ModelsEndpointGroup>();
    }

    public override async Task HandleAsync(CreateModelRequest req, CancellationToken ct)
    {
        ModelDto result = await ModelService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Model Created Successfully", ct);
    }
}