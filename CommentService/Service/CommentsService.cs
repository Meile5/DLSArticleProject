using CommentService.Clients;
using CommentService.Entities;
using CommentService.Repositories;
using ProfanityService.Models.Dtos;

namespace CommentService.Service;

public class CommentsService(CommentsRepo commentsRepo, IProfanityClient profanityClient)
{
    public async Task SaveComment(CommentDto commentDto, string userId)
    {
        var comment = new Comment
        {
            CommentId = Guid.NewGuid().ToString(),
            Text = commentDto.Comment,
        };
        var hasProfanity = await profanityClient.FilterComment(comment.Text);
        if (hasProfanity)
        {
            throw new Exception("Comment contains forbidden words");
        }
        await commentsRepo.SaveComment(comment, userId);
    }
}