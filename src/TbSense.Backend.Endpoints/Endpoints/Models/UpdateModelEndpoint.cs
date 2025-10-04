using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.Models;

public class UpdateModelEndpoint : BaseEndpoint<UpdateModelRequest, ModelDto>
{
    public IModelService ModelService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<ModelsEndpointGroup>();
    }

    public override async Task HandleAsync(UpdateModelRequest req, CancellationToken ct)
    {
        ModelDto result = await ModelService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Model Updated Successfully", ct);
    }
}