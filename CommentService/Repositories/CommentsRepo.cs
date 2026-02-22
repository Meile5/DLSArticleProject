using CommentService.Database;
using CommentService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Repositories;

public class CommentsRepo(AppDbContext dbContext)
{
    public async Task SaveComment(Comment comment, string userId){
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
            throw;
        }
    }

    public async Task<List<Comment>> GetComments(string articleId)
    {
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
            throw;
        }
         
    }
}