using Microsoft.EntityFrameworkCore;
using ProfanityService.Database;
using ProfanityService.Entities;

namespace ProfanityService.Repositories;

public class ProfanityRepo(AppDbContext dbContext)
{
    public async Task <List<Word>> GetWords(){
        try
        {
            var words = await dbContext.Words
                .ToListAsync();

            if (words == null)
            {
                throw new Exception("Words not found.");
            }

            return words;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}