using System.ComponentModel.DataAnnotations;

namespace NewsletterService.AppOptionsPattern;

public sealed class AppOptions
{
    [Required] public SubscriberServiceOptions SubscriberService { get; set; } = new();
}

public sealed class SubscriberServiceOptions
{
    [Required] public string BaseUrl { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 3;
}
