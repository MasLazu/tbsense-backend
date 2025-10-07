using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.KnowledgeBases;

public class GetKnowledgeBasesPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<KnowledgeBaseDto>>
{
    public IKnowledgeBaseService KnowledgeBaseService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<KnowledgeBasesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<KnowledgeBaseDto> result = await KnowledgeBaseService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "Knowledge Bases Paginated Retrieved Successfully", ct);
    }
}