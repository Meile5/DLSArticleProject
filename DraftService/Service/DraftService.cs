using DraftService.Models;
using DraftService.Repositories;

namespace DraftService.Service;

public class DraftService
{
    private readonly DraftRepository _repository;

    public DraftService(DraftRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateDraft(DraftDto request)
    {
        var draft = new Draft
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            AuthorId = request.AuthorId,
            StatusId = 1, // Draft
            CreatedAt = DateTime.UtcNow
        };

        await _repository.CreateAsync(draft);
    }

    public async Task<DraftDto> GetDraft(Guid id)
    {
        var draft = await _repository.GetByIdAsync(id);
        var draftDto = new DraftDto
        {
            Id = draft.Id,
            Title = draft.Title,
            AuthorId = draft.AuthorId,
            Content = draft.Content,
            CreatedAt = draft.CreatedAt,
            Status = (DraftStatusEnum)draft.StatusId
        };
        return draftDto;
        
    }

    public async Task UpdateDraft(Guid id, DraftDto request)
    {
        var draft = await _repository.GetByIdAsync(id);
        if (draft is null) throw new KeyNotFoundException($"Draft {id} not found");

        draft.Title = request.Title;
        draft.Content = request.Content;
        draft.StatusId = (int)request.Status;
        draft.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(draft);
    }

    public async Task DeleteDraft(Guid id)
    {
        var draft = await _repository.GetByIdAsync(id);
        if (draft is null) throw new KeyNotFoundException($"Draft {id} not found");

        await _repository.DeleteAsync(draft);
    }
}