using CommentService.Database;
using CommentService.Entities;
using Microsoft.EntityFrameworkCore;
using MonitorService;
using Serilog;

namespace CommentService.Repositories;

public class CommentsRepo(AppDbContext dbContext)
{
    public async Task SaveComment(Comment comment, string userId)
    {
        string shortenedComment = comment.Text.Substring(0, 15); //purely used for monitoring
        using var activity = Monitoring.ActivitySource.StartActivity("Entered SaveComment in CommentsRepo with user "+ userId + " & Comment " + shortenedComment);
        
        Log.Logger.Debug("Entered SaveComment in CommentsRepo with user {userId} & Comment {comment}", userId, shortenedComment);
        
        try
        {
            await dbContext.Comments.AddAsync(comment);
            
            var commentUser = new CommentUser
            {
                Id = Guid.NewGuid().ToString(),
                CommentId = comment.CommentId,
                UserId = userId
            };

            await dbContext.CommentUsers.AddAsync(commentUser);

            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Log.Logger.Error("Failed to save Comment");

            throw;
        }
    }

    public async Task<List<Comment>> GetComments(string articleId)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Entered GetComments in CommentsRepo");

        Log.Logger.Debug("Entered GetComments in CommentsRepo");
        
        try
        {
            return await dbContext.Comments
                .Include(c => c.CommentUsers)
                .Where(c => c.ArticleId == articleId)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Log.Logger.Error("Failed to get Comments");
            throw;
        }
         
    }
}