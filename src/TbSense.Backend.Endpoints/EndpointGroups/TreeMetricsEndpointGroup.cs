using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class TreeMetricsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public TreeMetricsEndpointGroup()
    {
        Configure("tree-metrics", ep => ep.Description(x => x.WithTags("Tree Metrics")));
    }
}