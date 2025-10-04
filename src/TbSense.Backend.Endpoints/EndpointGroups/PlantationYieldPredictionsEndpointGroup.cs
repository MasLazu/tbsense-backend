using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class PlantationYieldPredictionsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public PlantationYieldPredictionsEndpointGroup()
    {
        Configure("plantation-yield-predictions", ep => ep.Description(x => x.WithTags("Plantation Yield Predictions")));
    }
}