using Microsoft.AspNetCore.Mvc;

namespace AzureAiVision.Api.ObjectDetections;

public static class ObjectDetectionModule
{
    public static void RegisterObjectDetectionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/object-detection/upload-images", async (
            [FromQuery] Guid projectId,
            [FromServices] ObjectDetectionService service) =>
        {
            var result = await service.UploadImagesAsync(projectId);
            return Results.Ok(result);
        });
    }
} 