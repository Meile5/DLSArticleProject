namespace Shared;

public class SubscriberRemovedEvent : Event
{
    public Guid SubscriberId { get; set; }
}