using Shared;

namespace SubscriberQueue.Events;

public class NewSubscriberEvent : Event
{
    public Guid SubscriberId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime SubscribedAt { get; set; }
}