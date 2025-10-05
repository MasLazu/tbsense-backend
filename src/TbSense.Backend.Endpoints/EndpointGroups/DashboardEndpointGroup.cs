using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Microsoft.AspNetCore.Http;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class DashboardEndpointGroup : SubGroup<V1EndpointGroup>
{
    public DashboardEndpointGroup()
    {
        Configure("dashboard", ep => ep.Description(x => x.WithTags("Dashboard")));
    }
}
