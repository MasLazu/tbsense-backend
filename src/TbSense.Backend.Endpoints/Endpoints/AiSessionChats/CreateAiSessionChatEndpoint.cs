using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.AiSessionChats;

public class CreateAiSessionChatEndpoint : BaseEndpoint<CreateAiSessionChatRequest, AiSessionChatDto>
{
    public required IAiSessionChatService AiSessionChatService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<AiSessionChatsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateAiSessionChatRequest req, CancellationToken ct)
    {
        AiSessionChatDto result = await AiSessionChatService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "AI Session Chat Created Successfully", ct);
    }
}