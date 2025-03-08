using AzureAiVision.Api.ImageAnalyzisis;
using AzureAiVision.Api.ImageClassifications;
using AzureAiVision.Api.Models;
using AzureAiVision.Api.ObjectDetections;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.Configure<AzureCustomVision>(builder.Configuration.GetSection("AzureCustomVision"));
builder.Services.Configure<AzureComputerVision>(builder.Configuration.GetSection("AzureComputerVision"));

builder.Services.AddScoped<ImageAnalysisService>();
builder.Services.AddScoped<ImageClassificationService>();

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

app.Run();


