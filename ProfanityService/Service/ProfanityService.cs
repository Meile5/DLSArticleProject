using ProfanityService.Models.Dtos;
using ProfanityService.Repositories;

namespace ProfanityService.Service;

public class ProfanityService(ProfanityRepo profanityRepo)
{
    public async Task <bool> CheckForbiddenWords(CommentDto comment)
    {
        var words = await profanityRepo.GetWords();
        
        foreach(var word in words)
        {
            return comment.Comment.Contains(word.Word1);
        }
        return false;
    }
}