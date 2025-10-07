using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class AiSessionChatsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public AiSessionChatsEndpointGroup()
    {
        Configure("ai-session-chats", ep => ep.Description(x => x.WithTags("AI Session Chats")));
    }
}