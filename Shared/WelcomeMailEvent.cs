using Shared;

namespace SubscriberQueue.Events;

public class WelcomeMailEvent : Event
{
    public Guid SubscriberId { get; set; }
    public string Email { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}