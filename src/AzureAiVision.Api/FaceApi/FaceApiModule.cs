using Microsoft.AspNetCore.Mvc;

namespace AzureAiVision.Api.FaceApi;

public static class FaceApiModule
{
    public static void RegisterFaceApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/face/recognize", async (
                [FromServices] FaceApiService service) =>
            {
                await service.Recognize();
                return Results.Ok();
            })
            .WithTags("Face");
    }
}