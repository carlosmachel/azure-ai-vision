using AzureAiVision.Api.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ImageClassifications;

public class ProjectResult
{
    public Guid Id { get; set; }
}

public class ImageClassificationTrainingService(IOptions<AzureCustomTrainingVision> options)
{
    private CustomVisionTrainingClient Client { get; } = new(new ApiKeyServiceClientCredentials(options.Value.Key))
    {
        Endpoint = options.Value.Endpoint
    };
    
    public async Task<string> CheckStatusAsync(Guid projectId, Guid iterationId)
    {
        var iteration = await Client.GetIterationAsync(projectId, iterationId);
        return iteration.Status;
    }
    
    public async Task<Guid> TrainingAsync(Guid projectId)
    {
        //var project = await Client.GetProjectAsync(projectId);
        var result = await Client.GetProjectWithHttpMessagesAsync(projectId);
        
        UploadImages("images/training-images", result.Body.Id);
    
        var iteration =  await Client.TrainProjectAsync(projectId, forceTrain:true);
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