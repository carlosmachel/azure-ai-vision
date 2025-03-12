using Microsoft.AspNetCore.Mvc;

namespace AzureAiVision.Api.ImageAnalyzisis;

public static class ImageAnalysisModule
{
    public static void RegisterImageAnalysisEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/analyse-image/by-url", ([FromQuery] string url, [FromServices] ImageAnalysisService service) =>
            {
                //"https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png"
                var result = service.Analyze(url);
                return Results.Ok(result);
            })
            .WithName("AnalyzeImageByUrl");

        app.MapGet("/analyse-image/smart-thumbnail", async ([FromQuery] string url, [FromQuery] bool smartCropping, [FromServices] ImageAnalysisService service) =>
        {
            //https://images.pexels.com/photos/31000796/pexels-photo-31000796/free-photo-of-flying-indian-nightjar-in-natural-habitat.jpeg
            var result = await service.SmarthThumbnails(500, 500, url, smartCropping);
            return Results.File(result,  "image/jpeg", $"thumbnail{Guid.NewGuid()}.jpg");
        }).WithName("SmartThumbnail");
    }
}