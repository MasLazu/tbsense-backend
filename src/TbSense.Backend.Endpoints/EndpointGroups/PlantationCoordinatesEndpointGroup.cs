using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class PlantationCoordinatesEndpointGroup : SubGroup<V1EndpointGroup>
{
    public PlantationCoordinatesEndpointGroup()
    {
        Configure("plantation-coordinates", ep => ep.Description(x => x.WithTags("Plantation Coordinates")));
    }
}