using Azure;
using Azure.AI.Vision.ImageAnalysis;
using AzureAiVision.Api.Models;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ImageAnalyzisis;

public class ImageAnalysisService(IOptions<AzureComputerVision> options)
{
    private ImageAnalysisClient Client { get; } = new(new Uri(options.Value.VisionEndpoint),
        new AzureKeyCredential(options.Value.VisionKey));

    public AzureComputerVision Options { get; set; } = options.Value;

    public Response<ImageAnalysisResult> Analyze(string url)
    {
        var result = Client.Analyze(
            new Uri(url),
            VisualFeatures.People,
            new ImageAnalysisOptions { GenderNeutralCaption = true });

        return result;

    }
}