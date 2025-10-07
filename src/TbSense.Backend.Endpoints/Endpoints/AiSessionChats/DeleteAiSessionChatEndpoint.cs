using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessionChats;

public class DeleteAiSessionChatEndpoint : BaseEndpointWithoutResponse<IdRequest>
{
    public required IAiSessionChatService AiSessionChatService { get; set; }

    public override void ConfigureEndpoint()
    {
        Delete("/{Id}");
        Group<AiSessionChatsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        await AiSessionChatService.DeleteAsync(req.Id, true, ct);
        await SendOkResponseAsync("AI Session Chat Deleted Successfully", ct);
    }
}