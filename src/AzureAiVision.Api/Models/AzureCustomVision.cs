namespace AzureAiVision.Api.Models;

public class AzureCustomVision
{
    public string ProjectId { get; set; }
    public string TrainingEndpoint { get; set; }
    public string TrainingKey { get; set; }
}

public class AzureComputerVision
{
    public string VisionEndpoint { get; set; }
    public string VisionKey { get; set; }
}