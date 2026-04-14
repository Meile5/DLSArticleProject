using Shared.Events;
using SubscriberQueue.Events;

namespace Shared;

public class NewSubscriberSuccessEvent : Event
{
    public Guid SubscriberId { get; set; }
    public string Email { get; set; }

    public static NewSubscriberSuccessEvent FromSubscriberEvent(SubscriberCreatedEvent subEvent)
    {
        return new NewSubscriberSuccessEvent
        {
            SubscriberId = subEvent.SubscriberId,
            Email = subEvent.Email,
        };
    }
}