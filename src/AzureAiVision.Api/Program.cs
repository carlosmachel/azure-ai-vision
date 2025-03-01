using AzureAiVision.Api.Models;
using AzureAiVision.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using ApiKeyServiceClientCredentials = Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials;
using Tag = AzureAiVision.Api.Models.Tag;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<AzureCustomVision>(builder.Configuration.GetSection("AzureCustomVision"));
builder.Services.AddScoped<ImageAnalysisService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/analyse-image/by-url", (string url, [FromServices] ImageAnalysisService service) =>
    {
        //"https://learn.microsoft.com/azure/ai-services/computer-vision/media/quickstarts/presentation.png"
        var endpoint = Environment.GetEnvironmentVariable("VISION_ENDPOINT");
        var key = Environment.GetEnvironmentVariable("VISION_KEY");
        var result = service.Analyze(endpoint, key, url);
        return Results.Ok(result);
    })
    .WithName("AnalyzeImageByUrl");

app.MapGet("/image-classification/training", async ([FromServices] IOptions<AzureCustomVision> options) =>
{
    var client = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(options.Value.TrainingKey))
    {
        Endpoint = options.Value.TrainingEndpoint
    };
    
    var project = await client.GetProjectAsync(Guid.Parse(options.Value.ProjectId));

    UploadImages("images/training-images", client, project);
    
    var iteration =  client.TrainProject(project.Id);

    return Results.Ok(iteration);
});

app.MapGet("/image-classification/status", async (Guid iterationId, [FromServices] IOptions<AzureCustomVision> options) =>
{
    var client = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(options.Value.TrainingKey))
    {
        Endpoint = options.Value.TrainingEndpoint
    };

    var iteration = await client.GetIterationAsync(Guid.Parse(options.Value.ProjectId), iterationId);

    return Results.Ok(iteration.Status);
});

app.Run();
return;

void UploadImages(string folder, CustomVisionTrainingClient client, Project project)
{
    var tags = client.GetTags(project.Id);
    foreach (var tag in tags)
    {
        var images = Directory.GetFiles(Path.Combine(folder, tag.Name));
        foreach (var image in images)
        {
            using var stream = new MemoryStream(File.ReadAllBytes(image));
            client.CreateImagesFromData(project.Id, stream, [tag.Id]);
        }
    }
}