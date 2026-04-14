namespace SubscriberService.Entities;

public class Subscriber
{
    public Guid SubscriberId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime SubscribedAt { get; set; }
    public bool IsActive { get; set; }
}