using SubscriberService.Database;
using SubscriberService.Dtos;
using SubscriberService.Entities;
using ArticleQueue.Interfaces;
using Shared.Events;

namespace SubscriberService.Services;

public class SubscriberService : ISubscriberService
{
    private readonly ISubscriberRepository _repository;
    private readonly IMessageClient _messageClient;

    public SubscriberService(
        ISubscriberRepository repository,
        IMessageClient messageClient)
    {
        _repository = repository;
        _messageClient = messageClient;
    }

    public async Task<SubscriberReadDto> SubscribeAsync(SubscriberCreateDto dto)
    {
        var existing = await _repository.GetByEmailAsync(dto.Email);

        if (existing != null && existing.IsActive)
            throw new Exception("Already subscribed");

        var subscriber = new Subscriber
        {
            SubscriberId = Guid.NewGuid(),
            Email = dto.Email,
            SubscribedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _repository.CreateAsync(subscriber);

        await _messageClient.Publish(new NewSubscriberSuccessEvent
        {
            SubscriberId = subscriber.SubscriberId,
            Email = subscriber.Email
        });

        return new SubscriberReadDto
        {
            SubscriberId = subscriber.SubscriberId,
            Email = subscriber.Email
        };
    }

    public async Task<bool> UnsubscribeAsync(string email)
    {
        var existing = await _repository.GetByEmailAsync(email);

        if (existing == null || !existing.IsActive)
            return false;

        await _repository.UnsubscribeAsync(email);

        await _messageClient.Publish(new SubscriberUnsubscribedEvent
        {
            Email = email
        });

        return true;
    }

    public async Task<IEnumerable<SubscriberReadDto>> GetSubscribersAsync()
    {
        var subs = await _repository.GetAllActiveAsync();

        return subs.Select(s => new SubscriberReadDto
        {
            SubscriberId = s.SubscriberId,
            Email = s.Email
        });
    }
}