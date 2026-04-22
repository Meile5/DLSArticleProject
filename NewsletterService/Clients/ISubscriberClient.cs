using NewsletterService.Models.Dtos;

namespace NewsletterService.Clients;

public interface ISubscriberClient
{
    Task<IReadOnlyList<SubscribersDto>> GetSubscribersAsync(CancellationToken ct = default);
}
