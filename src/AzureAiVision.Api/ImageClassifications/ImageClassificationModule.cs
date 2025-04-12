using AzureAiVision.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ImageClassifications;

public static class ImageClassificationModule
{
    public static void RegisterImageClassificationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/image-classification/training", async (
            [FromQuery] Guid projectId,
            [FromServices] ImageClassificationTrainingService service) =>
        {
            var iterationId = await service.TrainingAsync(projectId);
            return Results.Ok(iterationId);
        })
        .WithTags("Image Classification");

        app.MapGet("/image-classification/status", async (
            [FromQuery] Guid projectId, 
            [FromQuery] Guid iterationId, 
            [FromServices] ImageClassificationTrainingService service) =>
        {
            var status = await service.CheckStatusAsync(projectId, iterationId);
            return Results.Ok(status);
        })
        .WithTags("Image Classification");
        
        app.MapGet("/image-classification/classify", async (
            [FromQuery] Guid projectId,
            [FromQuery] string projectName,
            [FromQuery] string url,
            [FromServices] ImageClassificationPredictionService service) =>
        {
            var iterationId =  await service.ClassifyAsync(projectId, projectName, url);
            return Results.Ok(iterationId);
        })
        .WithTags("Image Classification");
    }
}