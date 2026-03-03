using CommentService.Database;
using CommentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Repositories;

public class CommentsRepo(AppDbContext dbContext)
{
    public async Task SaveComment(Comment comment, string userId)
    {
        using var activity = MonitorService.MonitorService.ActivitySource.StartActivity();
        
        MonitorService.MonitorService.Log.Debug("Entered SaveComment in CommentsRepo with user {userId} & Comment {comment}", userId, comment.Text.Substring(0,15));
        
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
            MonitorService.MonitorService.Log.Error("Failed to save Comment");

            throw;
        }
    }

    public async Task<List<Comment>> GetComments(string articleId)
    {
        using var activity = MonitorService.MonitorService.ActivitySource.StartActivity();

        MonitorService.MonitorService.Log.Debug("Entered GetComments in CommentsRepo");
        
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
            MonitorService.MonitorService.Log.Error("Failed to get Comments");
            throw;
        }
         
    }
}