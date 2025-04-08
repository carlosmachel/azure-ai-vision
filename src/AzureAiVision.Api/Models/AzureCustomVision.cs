namespace AzureAiVision.Api.Models;

public class AzureCustomVision
{
    public string Endpoint { get; set; }
    public string Key { get; set; }
}

public class AzureCustomTrainingVision : AzureCustomVision;
public class AzureCustomPredictionVision : AzureCustomVision;

public class AzureComputerVision
{
    public string VisionEndpoint { get; set; }
    public string VisionKey { get; set; }
}