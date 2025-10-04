using FastEndpoints;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Endpoints.TreeMetrics;

public class CreateTreeMetricEndpoint : BaseEndpoint<CreateTreeMetricRequest, TreeMetricDto>
{
    public ITreeMetricService TreeMetricService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("/");
        Group<TreeMetricsEndpointGroup>();
    }

    public override async Task HandleAsync(CreateTreeMetricRequest req, CancellationToken ct)
    {
        TreeMetricDto result = await TreeMetricService.CreateAsync(req, true, ct);
        await SendOkResponseAsync(result, "Tree Metric Created Successfully", ct);
    }
}