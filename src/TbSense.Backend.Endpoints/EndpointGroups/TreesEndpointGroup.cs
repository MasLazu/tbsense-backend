using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class TreesEndpointGroup : SubGroup<V1EndpointGroup>
{
    public TreesEndpointGroup()
    {
        Configure("trees", ep => ep.Description(x => x.WithTags("Trees")));
    }
}