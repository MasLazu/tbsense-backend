using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.KnowledgeBases;

public class GetKnowledgeBaseByIdEndpoint : BaseEndpoint<IdRequest, KnowledgeBaseDto>
{
    public required IKnowledgeBaseService KnowledgeBaseService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<KnowledgeBasesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        KnowledgeBaseDto? result = await KnowledgeBaseService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(KnowledgeBaseDto), req.Id);
        await SendOkResponseAsync(result, "Knowledge Base Retrieved Successfully", ct);
    }
}