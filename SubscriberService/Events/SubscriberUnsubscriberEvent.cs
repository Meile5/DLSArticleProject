namespace SubscriberService.Events;

public class SubscriberUnsubscribedEvent
{
    public string Email { get; set; } = string.Empty;
}