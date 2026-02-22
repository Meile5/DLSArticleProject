using CommentService.Database;
using CommentService.Entities;

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
    
}