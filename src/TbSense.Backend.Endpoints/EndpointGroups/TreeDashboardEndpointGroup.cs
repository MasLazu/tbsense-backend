using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Microsoft.AspNetCore.Http;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class TreeDashboardEndpointGroup : SubGroup<V1EndpointGroup>
{
    public TreeDashboardEndpointGroup()
    {
        Configure("dashboard/trees", ep => ep.Description(x => x.WithTags("Tree Dashboard")));
    }
}
