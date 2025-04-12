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
            .WithName("AnalyzeImageByUrl")
            .WithTags("Analyze Image");
        
        app.MapPost("/analyse-image/by-upload", async (IFormFile image, [FromServices] ImageAnalysisService service) =>
            {
                if (image == null || image.Length == 0)
                {
                    return Results.BadRequest("Arquivo inválido.");
                }

                using var memoryStream = new MemoryStream();
                await image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                var binaryData = await BinaryData.FromStreamAsync(memoryStream);
                var result = service.Analyze(binaryData);
                
                return Results.Ok(result);
            })
            .DisableAntiforgery()
            .WithName("AnalyzeImageByUpload")
            .WithTags("Analyze Image");

        app.MapGet("/analyse-image/smart-thumbnail", async ([FromQuery] string url, [FromQuery] bool smartCropping, [FromServices] ImageAnalysisService service) =>
        {
            //https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2
            var result = await service.SmarthThumbnails(100, 100, url, smartCropping);
            return Results.File(result,  "image/jpeg", $"thumbnail{Guid.NewGuid()}.jpg");
        }).WithName("SmartThumbnail")
        .WithTags("Analyze Image");
    }
}