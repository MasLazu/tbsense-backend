using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class SystemPromptsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public SystemPromptsEndpointGroup()
    {
        Configure("system-prompts", ep => ep.Description(x => x.WithTags("System Prompts")));
    }
}