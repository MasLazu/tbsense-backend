using FastEndpoints;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Application.Models;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Abstraction.Models;
using TbSense.Backend.Endpoints.EndpointGroups;

namespace TbSense.Backend.Endpoints.Models;

public class DownloadModelEndpoint : Endpoint<IdRequest>
{
    public IModelService ModelService { get; set; }

    public override void Configure()
    {
        Get("/{Id}/download");
        Group<ModelsEndpointGroup>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(IdRequest req, CancellationToken ct)
    {
        (Stream stream, string fileName) = await ModelService.DownloadModelAsync(req.Id, ct);

        await Send.StreamAsync(
            stream: stream,
            fileName: fileName,
            fileLengthBytes: stream.Length,
            contentType: "application/octet-stream",
            cancellation: ct);
    }
}
