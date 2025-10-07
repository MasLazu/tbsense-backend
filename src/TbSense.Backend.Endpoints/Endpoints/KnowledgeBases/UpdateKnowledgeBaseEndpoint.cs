using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.KnowledgeBases;

public class UpdateKnowledgeBaseEndpoint : BaseEndpoint<UpdateKnowledgeBaseRequest, KnowledgeBaseDto>
{
    public IKnowledgeBaseService KnowledgeBaseService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<KnowledgeBasesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateKnowledgeBaseRequest req, CancellationToken ct)
    {
        KnowledgeBaseDto result = await KnowledgeBaseService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Knowledge Base Updated Successfully", ct);
    }
}