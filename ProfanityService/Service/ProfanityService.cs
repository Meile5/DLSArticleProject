using MonitorService;
using ProfanityService.Models.Dtos;
using ProfanityService.Repositories;
using Serilog;

namespace ProfanityService.Service;

public class ProfanityService(ProfanityRepo profanityRepo)
{
    public async Task <bool> CheckForbiddenWords(CommentDto comment)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Entered CheckForbiddenWords in ProfanityService");
        
        Log.Logger.Debug("Entered CheckForbiddenWords in ProfanityService");
        
        var words = await profanityRepo.GetWords();
        
        foreach(var word in words)
        {
            if (comment.Comment.Contains(word.Word1, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}