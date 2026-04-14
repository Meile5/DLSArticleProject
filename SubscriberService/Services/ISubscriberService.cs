using SubscriberService.Dtos;

namespace SubscriberService.Services;

public interface ISubscriberService
{
    Task<SubscriberReadDto> SubscribeAsync(SubscriberCreateDto dto);
    Task<bool> UnsubscribeAsync(string email);
    Task<IEnumerable<SubscriberReadDto>> GetSubscribersAsync();
}