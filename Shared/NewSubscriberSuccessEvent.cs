using SubscriberQueue.Events;

namespace Shared;

public class NewSubscriberSuccessEvent : Event
{
    public Guid SubscriberId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime SubscribedAt { get; set; }

    public static NewSubscriberSuccessEvent FromSubscriberEvent(NewSubscriberEvent subEvent)
    {
        return new NewSubscriberSuccessEvent
        {
            SubscriberId = subEvent.SubscriberId,
            Username = subEvent.Username,
            Email = subEvent.Email,
            SubscribedAt = subEvent.SubscribedAt
        };
    }
}