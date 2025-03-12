using Azure;
using Azure.AI.Vision.ImageAnalysis;
using AzureAiVision.Api.Models;
using Microsoft.Extensions.Options;

namespace AzureAiVision.Api.ImageAnalyzisis;

public class ImageAnalysisService(IHttpClientFactory httpClientFactory, IOptions<AzureComputerVision> options)
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

    public async Task<byte[]> SmarthThumbnails(int width, int height, string url, bool smartCropping = false)
    {
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(options.Value.VisionEndpoint);
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", options.Value.VisionKey);
        var result = await client.PostAsJsonAsync($"vision/v3.2/generateThumbnail?width={width}&height={height}&smartCropping={smartCropping}", new
        {
            url
        });
        return await result.Content.ReadAsByteArrayAsync();
        
    }
}