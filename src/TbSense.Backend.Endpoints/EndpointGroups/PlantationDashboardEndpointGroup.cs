using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Microsoft.AspNetCore.Http;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class PlantationDashboardEndpointGroup : SubGroup<V1EndpointGroup>
{
    public PlantationDashboardEndpointGroup()
    {
        Configure("dashboard/plantations", ep => ep.Description(x => x.WithTags("Plantation Dashboard")));
    }
}
