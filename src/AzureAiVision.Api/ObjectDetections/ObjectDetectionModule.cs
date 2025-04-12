using Microsoft.AspNetCore.Mvc;

namespace AzureAiVision.Api.ObjectDetections;

public static class ObjectDetectionModule
{
    public static void RegisterObjectDetectionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/object-detection/upload-images", async (
            [FromQuery] Guid projectId,
            [FromServices] ObjectDetectionTrainingService service) =>
        {
            var result = await service.UploadImagesAsync(projectId);
            return Results.Ok(result);
        })
        .WithTags("Object Detection");
        
        app.MapGet("/object-detection/training", async (
            [FromQuery] Guid projectId,
            [FromServices] ObjectDetectionTrainingService service) =>
        {
            var result = await service.TrainingAsync(projectId);
            return Results.Ok(result);
        })
        .WithTags("Object Detection");
        
        app.MapGet("/object-detection/classify", async (
            [FromQuery] Guid projectId,
            [FromQuery] string projectName,
            [FromQuery] string url,
            [FromServices] ObjectDetectionPredictionService service) =>
        {
            var iterationId = service.ClassifyAsync(projectId, projectName, url);
            return Results.Ok(iterationId);
        })
        .WithTags("Object Detection");
    }
} 