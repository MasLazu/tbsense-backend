using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.TreeMetrics;

public class UpdateTreeMetricEndpoint : BaseEndpoint<UpdateTreeMetricRequest, TreeMetricDto>
{
    public ITreeMetricService TreeMetricService { get; set; }

    public override void ConfigureEndpoint()
    {
        Put("/");
        Group<TreeMetricsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateTreeMetricRequest req, CancellationToken ct)
    {
        TreeMetricDto result = await TreeMetricService.UpdateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Tree Metric Updated Successfully", ct);
    }
}