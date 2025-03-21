using AzureAiVision.Api.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ImageClassifications;

public class ImageClassificationService(IOptions<AzureCustomVision> options)
{
    private CustomVisionTrainingClient Client { get; } = new(new ApiKeyServiceClientCredentials(options.Value.TrainingKey))
    {
        Endpoint = options.Value.TrainingEndpoint
    };
    
    public async Task<string> CheckStatusAsync(Guid projectId, Guid iterationId)
    {
        var iteration = await Client.GetIterationAsync(projectId, iterationId);
        return iteration.Status;
    }
    
    public async Task<Guid> TrainingAsync(Guid projectId)
    {
        var project = await Client.GetProjectAsync(projectId);

        UploadImages("images/training-images", project.Id);
    
        var iteration =  await Client.TrainProjectAsync(project.Id);
        return iteration.Id;
        
    }

    private void UploadImages(string folder, Guid projectId)
    {
        var tags = Client.GetTags(projectId);
        foreach (var tag in tags)
        {
            var images = Directory.GetFiles(Path.Combine(folder, tag.Name));
            foreach (var image in images)
            {
                using var stream = new MemoryStream(File.ReadAllBytes(image));
                Client.CreateImagesFromData(projectId, stream, [tag.Id]);
            }
        }
    }
}
