using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class KnowledgeBasesEndpointGroup : SubGroup<V1EndpointGroup>
{
    public KnowledgeBasesEndpointGroup()
    {
        Configure("knowledge-bases", ep => ep.Description(x => x.WithTags("Knowledge Bases")));
    }
}