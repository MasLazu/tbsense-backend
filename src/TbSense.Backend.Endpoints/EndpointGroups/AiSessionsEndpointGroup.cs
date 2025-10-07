using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class AiSessionsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public AiSessionsEndpointGroup()
    {
        Configure("ai-sessions", ep => ep.Description(x => x.WithTags("AI Sessions")));
    }
}