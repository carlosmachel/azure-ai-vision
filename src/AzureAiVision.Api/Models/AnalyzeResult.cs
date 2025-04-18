namespace AzureAiVision.Api.Models;

public record AnalyzeResult(IEnumerable<Caption> Captions);

public record Caption(string Text, string Confidence);

public record ImageAnalysisTag();

public record Category();

public record DetectedBrand();

public record DetectedObjected();

public record ModerationRatings();