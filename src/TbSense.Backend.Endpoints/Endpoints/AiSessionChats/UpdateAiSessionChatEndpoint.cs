using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessionChats;

public class UpdateAiSessionChatEndpoint : BaseEndpoint<UpdateAiSessionChatRequest, AiSessionChatDto>
{
    public required IAiSessionChatService AiSessionChatService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<AiSessionChatsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateAiSessionChatRequest req, CancellationToken ct)
    {
        AiSessionChatDto result = await AiSessionChatService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "AI Session Chat Updated Successfully", ct);
    }
}