namespace SubscriberService.AppOptionsPattern;

public sealed class AppOptions
{
    public string DbConnectionString { get; set; } = string.Empty!;
    public string RabbitMqHost { get; set; } = string.Empty!;
}