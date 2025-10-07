using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;
using MasLazu.AspNet.Framework.Application.Exceptions;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessionChats;

public class GetAiSessionChatByIdEndpoint : BaseEndpoint<IdRequest, AiSessionChatDto>
{
    public required IAiSessionChatService AiSessionChatService { get; set; }

    public override void ConfigureEndpoint()
    {
        Get("/{Id}");
        Group<AiSessionChatsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        AiSessionChatDto? result = await AiSessionChatService.GetByIdAsync(req.Id, ct) ??
            throw new NotFoundException(nameof(AiSessionChatDto), req.Id);
        await SendOkResponseAsync(result, "AI Session Chat Retrieved Successfully", ct);
    }
}