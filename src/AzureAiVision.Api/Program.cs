using AzureAiVision.Api.FaceApi;
using AzureAiVision.Api.ImageAnalyzisis;
using AzureAiVision.Api.ImageClassifications;
using AzureAiVision.Api.Models;
using AzureAiVision.Api.ObjectDetections;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.Configure<AzureCustomPredictionVision>(builder.Configuration.GetSection("AzureCustomPredictionVision"));
builder.Services.Configure<AzureCustomTrainingVision>(builder.Configuration.GetSection("AzureCustomTrainingVision"));
builder.Services.Configure<AzureComputerVision>(builder.Configuration.GetSection("AzureComputerVision"));
builder.Services.Configure<AzureFaceApi>(builder.Configuration.GetSection("AzureFaceApi"));

builder.Services.AddScoped<ImageAnalysisService>();
builder.Services.AddScoped<ImageClassificationPredictionService>();
builder.Services.AddScoped<ImageClassificationTrainingService>();
builder.Services.AddScoped<FaceApiService>();

builder.Services.AddScoped<ObjectDetectionPredictionService>();
builder.Services.AddScoped<ObjectDetectionTrainingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.RegisterImageAnalysisEndpoints();
app.RegisterImageClassificationEndpoints();
app.RegisterObjectDetectionEndpoints();
app.RegisterFaceApiEndpoints();
app.Run();


