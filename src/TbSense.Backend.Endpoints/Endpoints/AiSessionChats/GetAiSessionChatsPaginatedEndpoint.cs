using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessionChats;

public class GetAiSessionChatsPaginatedEndpoint : BaseEndpoint<PaginationRequest, PaginatedResult<AiSessionChatDto>>
{
    public IAiSessionChatService AiSessionChatService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/paginated");
        Group<AiSessionChatsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(PaginationRequest req, CancellationToken ct)
    {
        PaginatedResult<AiSessionChatDto> result = await AiSessionChatService.GetPaginatedAsync(req, ct);
        await SendOkResponseAsync(result, "AI Session Chats Paginated Retrieved Successfully", ct);
    }
}