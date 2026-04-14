namespace SubscriberService.Dtos;

public class SubscriberReadDto
{
    public Guid SubscriberId { get; set; }
    public string Email { get; set; } = string.Empty;
}