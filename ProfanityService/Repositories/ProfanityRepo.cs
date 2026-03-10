using Microsoft.EntityFrameworkCore;
using MonitorService;
using ProfanityService.Database;
using ProfanityService.Entities;
using Serilog;

namespace ProfanityService.Repositories;

public class ProfanityRepo(AppDbContext dbContext)
{
    public async Task <List<Word>> GetWords()
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Entered GetWords in ProfanityRepo");
        
        Log.Logger.Debug("Entered GetWords in ProfanityRepo");
        
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