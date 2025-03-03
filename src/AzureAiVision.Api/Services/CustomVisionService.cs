using AzureAiVision.Api.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.Services;

public class CustomVisionService(IOptions<AzureCustomVision> options)
{
    private CustomVisionTrainingClient Client { get; } = new(new ApiKeyServiceClientCredentials(options.Value.TrainingKey))
    {
        Endpoint = options.Value.TrainingEndpoint
    };

    private AzureCustomVision Options { get; } = options.Value;

    public async Task<string> CheckStatusAsync(Guid iterationId)
    {
        var iteration = await Client.GetIterationAsync(Guid.Parse(Options.ProjectId), iterationId);
        return iteration.Status;
    }
    
    public async Task<Guid> TrainingAsync()
    {
        var project = await Client.GetProjectAsync(Guid.Parse(Options.ProjectId));

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
