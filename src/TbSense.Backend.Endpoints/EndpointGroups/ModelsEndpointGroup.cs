using FastEndpoints;
using Microsoft.AspNetCore.Http;
using MasLazu.AspNet.Framework.Endpoint.EndpointGroups;

namespace TbSense.Backend.Endpoints.EndpointGroups;

public class ModelsEndpointGroup : SubGroup<V1EndpointGroup>
{
    public ModelsEndpointGroup()
    {
        Configure("models", ep => ep.Description(x => x.WithTags("Models")));
    }
}