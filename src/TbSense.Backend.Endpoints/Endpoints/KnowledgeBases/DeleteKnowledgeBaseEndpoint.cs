using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.KnowledgeBases;

public class DeleteKnowledgeBaseEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public required IKnowledgeBaseService KnowledgeBaseService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<KnowledgeBasesEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await KnowledgeBaseService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("Knowledge Base Deleted Successfully", ct);
    }
}