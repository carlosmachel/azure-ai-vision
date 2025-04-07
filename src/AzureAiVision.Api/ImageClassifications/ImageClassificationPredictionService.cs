using AzureAiVision.Api.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ImageClassifications;

public class ImageClassificationPredictionService(IOptions<AzureCustomVision> options)
{
    private CustomVisionPredictionClient Client { get; } = new(
        new ApiKeyServiceClientCredentials(
            options.Value.TrainingKey));
    
    public async Task<ImagePrediction> ClassifyAsync(Guid projectId, string publishedName, string url)
    {
        var prediction = await Client.ClassifyImageUrlAsync(projectId, publishedName, new ImageUrl(url));
        return prediction;
    }
    
   
}
