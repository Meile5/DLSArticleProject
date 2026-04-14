using SubscriberService.Entities;

namespace SubscriberService.Database;

public interface ISubscriberRepository
{
    Task<Subscriber> CreateAsync(Subscriber subscriber);
    Task<Subscriber?> GetByEmailAsync(string email);
    Task<IEnumerable<Subscriber>> GetAllActiveAsync();
    Task UnsubscribeAsync(string email);
}