namespace DraftService.Repositories;

using DraftService.Database;
using DraftService.Models;
using Microsoft.EntityFrameworkCore;

public class DraftRepository
{
    private readonly DraftContext _context;

    public DraftRepository(DraftContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Draft draft)
    {
        _context.Drafts.Add(draft);
        await _context.SaveChangesAsync();
    }

    public async Task<Draft?> GetByIdAsync(Guid id)
    {
        return await _context.Drafts
            .Include(d => d.Status)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task UpdateAsync(Draft draft)
    {
        _context.Drafts.Update(draft);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Draft draft)
    {
        _context.Drafts.Remove(draft);
        await _context.SaveChangesAsync();
    }
}