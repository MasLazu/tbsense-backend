using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.KnowledgeBases;

public class CreateKnowledgeBaseEndpoint : BaseEndpoint<CreateKnowledgeBaseRequest, KnowledgeBaseDto>
{
    public required IKnowledgeBaseService KnowledgeBaseService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<KnowledgeBasesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateKnowledgeBaseRequest req, CancellationToken ct)
    {
        KnowledgeBaseDto result = await KnowledgeBaseService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Knowledge Base Created Successfully", ct);
    }
}