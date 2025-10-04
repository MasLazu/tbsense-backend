using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class PlantationsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public PlantationsEndpointGroup()
    {
        Configure("plantations", ep => ep.Description(x => x.WithTags("Plantations")));
    }
}