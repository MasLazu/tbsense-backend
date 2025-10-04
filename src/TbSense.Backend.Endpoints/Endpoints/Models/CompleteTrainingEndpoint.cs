using FastEndpoints;
using MasLazu.AspNet.Framework.Application.Exceptions;
using MasLazu.AspNet.Framework.Endpoint.Endpoints;
using Microsoft.AspNetCore.Http;
using TbSense.Backend.Abstraction.Interfaces;
using TbSense.Backend.Endpoints.EndpointGroups;
using TbSense.Backend.Endpoints.Models;

namespace TbSense.Backend.Endpoints.Endpoints.Models;

public class CompleteTrainingEndpoint : BaseEndpointWithoutResponse<CompleteTrainingRequest>
{
    public IModelService ModelService { get; set; }

    public override void ConfigureEndpoint()
    {
        Post("{modelId}/training-complete");
        AllowAnonymous();
        AllowFileUploads();
        Group<ModelsEndpointGroup>();
    }

    public override async Task HandleAsync(CompleteTrainingRequest req, CancellationToken ct)
    {
        if (req.ModelFile == null || req.ModelFile.Length == 0)
        {
            throw new BadRequestException("Model file is required");
        }

        await using Stream fileStream = req.ModelFile.OpenReadStream();

        await ModelService.CompleteTrainingAsync(
            req.ModelId,
            fileStream,
            req.ModelFile.FileName,
            req.Accuracy,
            req.MAE,
            req.RMSE,
            req.R2Score,
            ct);

        await SendOkResponseAsync("", ct);
    }
}
