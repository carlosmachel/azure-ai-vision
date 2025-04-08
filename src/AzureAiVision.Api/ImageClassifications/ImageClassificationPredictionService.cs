using AzureAiVision.Api.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ImageClassifications;

public class ImageClassificationPredictionService(IHttpClientFactory httpClientFactory, IOptions<AzureCustomPredictionVision> options)
{
    private CustomVisionPredictionClient Client { get; } = new(new ApiKeyServiceClientCredentials(options.Value.Key))
    {
        Endpoint = options.Value.Endpoint
    };

    public async Task<ImagePrediction> ClassifyAsync(Guid projectId, string publishedName, string url)
    {
        var prediction =
            await Client.ClassifyImageUrlAsync(projectId, publishedName, new ImageUrl(url));
        return prediction;
    }
}
