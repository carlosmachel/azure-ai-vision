using Azure;
using Azure.AI.Vision.ImageAnalysis;

namespace AzureAiVision.Api.Services;

public class ImageAnalysisService
{
    public Response<ImageAnalysisResult> Analyze(string endpoint, string key, string url)
    {
        var client = new ImageAnalysisClient(
            new Uri(endpoint),
            new AzureKeyCredential(key));

        var result = client.Analyze(
            new Uri(url),
            VisualFeatures.People,
            new ImageAnalysisOptions { GenderNeutralCaption = true });

        return result;

    }
}