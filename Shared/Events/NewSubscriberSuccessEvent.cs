namespace Shared.Events;

public class NewSubscriberSuccessEvent : Event
{
    public Guid SubscriberId { get; set; }
    public string Email { get; set; }
}