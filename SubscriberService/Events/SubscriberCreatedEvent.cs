namespace SubscriberService.Events;

public class SubscriberCreatedEvent
{
    public Guid SubscriberId { get; set; }
    public string Email { get; set; } = string.Empty;
}