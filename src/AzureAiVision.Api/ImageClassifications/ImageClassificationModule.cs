using Microsoft.AspNetCore.Mvc;

namespace AzureAiVision.Api.ImageClassifications;

public static class ImageClassificationModule
{
    public static void RegisterImageClassificationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/image-classification/training", async (
            [FromQuery] Guid projectId,
            [FromServices] ImageClassificationService service) =>
        {
            var iterationId = service.TrainingAsync(projectId);
            return Results.Ok(iterationId);
        });

        app.MapGet("/image-classification/status", async (
            [FromQuery] Guid projectId, 
            [FromQuery] Guid iterationId, 
            [FromServices] ImageClassificationService service) =>
        {
            var status = await service.CheckStatusAsync(projectId, iterationId);
            return Results.Ok(status);
        });
    }
}