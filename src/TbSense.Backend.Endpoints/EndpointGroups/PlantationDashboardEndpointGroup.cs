using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;
using Microsoft.AspNetCore.Http;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class PlantationDashboardEndpointGroup : SubGroup<DashboardEndpointGroup>
{
    public PlantationDashboardEndpointGroup()
    {
        Configure("plantations/{PlantationId}", ep => ep.Description(x => x.WithTags("Plantation Dashboard")));
    }
}
