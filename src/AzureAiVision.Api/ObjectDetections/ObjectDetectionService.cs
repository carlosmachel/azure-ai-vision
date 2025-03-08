using System.Text.Json;
using AzureAiVision.Api.Models;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ObjectDetections;

public class ObjectDetectionService(IOptions<AzureCustomVision> options)
{
    private CustomVisionTrainingClient Client { get; } =
        new(new ApiKeyServiceClientCredentials(options.Value.TrainingKey))
        {
            Endpoint = options.Value.TrainingEndpoint
        };


    public async Task<ImageCreateSummary> UploadImagesAsync(Guid projectId)
    {
        return await UploadImages("images/object-detection-training-images", projectId);
    }

    private async Task<ImageCreateSummary> UploadImages(string folder, Guid projectId)
    {
        //Pega as tags definidas no projeto
        IList<Tag> tags = await Client.GetTagsAsync(projectId);

        //Criar a lista de images com as regioes marcadas.
        var imageFileEntries = new List<ImageFileCreateEntry>();

        var tagJson = await File.ReadAllTextAsync("images/object-detection-training-images/tagged-images.json");
        using var document = JsonDocument.Parse(tagJson);
        var files = document.RootElement.GetProperty("files");
        List<Region> regions = [];
        foreach (var file in files.EnumerateArray())
        {
            // Get the filename
            var filename = file.GetProperty("filename").GetString();

            // Get the tagged regions
            var tagged_regions = file.GetProperty("tags");
            foreach (var tag in tagged_regions.EnumerateArray())
            {
                var tagName = tag.GetProperty("tag").GetString();
                // Look up the tag ID for this tag name
                var tagItem = tags.FirstOrDefault(t => t.Name == tagName);
                var tagId = tagItem.Id;
                var left = tag.GetProperty("left").GetDouble();
                var top = tag.GetProperty("top").GetDouble();
                var width = tag.GetProperty("width").GetDouble();
                var height = tag.GetProperty("height").GetDouble();
                // Add a region for this tag using the coordinates and dimensions in the JSON
                regions.Add(new Region(tagId, left, top, width, height));
            }

            // Add the image and its regions to the list
            imageFileEntries.Add(
                new ImageFileCreateEntry(filename,
                    await File.ReadAllBytesAsync(Path.Combine(folder, filename)), null, regions));

        }
        
        var result = await Client.CreateImagesFromFilesAsync(projectId, new ImageFileCreateBatch(imageFileEntries));
        return result;
    }
}