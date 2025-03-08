using Microsoft.AspNetCore.Mvc;

namespace AzureAiVision.Api.ImageAnalyzisis;

public static class ImageAnalysisModule
{
    public static void RegisterImageAnalysisEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/analyse-image/by-url", (string url, [FromServices] ImageAnalysisService service) =>
            {
                //"https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png"
                var result = service.Analyze(url);
                return Results.Ok(result);
            })
            .WithName("AnalyzeImageByUrl");
    }
}