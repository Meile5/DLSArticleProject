namespace NewsletterService.Models.Dtos;

public class SubscribersDto
{
    public Guid SubscriberId { get; set; }
    public required string Email { get; set; }
}
