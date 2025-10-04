using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class PlantationHarvestsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public PlantationHarvestsEndpointGroup()
    {
        Configure("plantation-harvests", ep => ep.Description(x => x.WithTags("Plantation Harvests")));
    }
}