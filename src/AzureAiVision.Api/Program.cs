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
builder.Services.Configure<AzureComputerVision>(builder.Configuration.GetSection("AzureComputerVision"));

builder.Services.AddScoped<ImageAnalysisService>();
builder.Services.AddScoped<CustomVisionService>();

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
        var result = service.Analyze(url);
        return Results.Ok(result);
    })
    .WithName("AnalyzeImageByUrl");

app.MapGet("/image-classification/training", async ([FromServices] CustomVisionService service) =>
{
    var iterationId = service.TrainingAsync();
    return Results.Ok(iterationId);
});

app.MapGet("/image-classification/status", async (Guid iterationId, [FromServices] CustomVisionService service) =>
{
    var status = await service.CheckStatusAsync(iterationId);
    return Results.Ok(status);
});

app.Run();


