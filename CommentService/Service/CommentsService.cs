using CommentService.Clients;
using CommentService.Entities;
using CommentService.Models.Dtos;
using CommentService.Repositories;

namespace CommentService.Service;

public class CommentsService(CommentsRepo commentsRepo, IProfanityClient profanityClient)
{
    public async Task SaveComment(CreateCommentDto createCommentDto)
    {
        //for OpenTelemetry/Zipkin
        using var activity = MonitorService.MonitorService.ActivitySource.StartActivity();
        
        //for Serilog debugging
        MonitorService.MonitorService.Log.Debug("Entered SaveComment in CommentsService");

        
        var comment = new Comment
        {
            CommentId = Guid.NewGuid().ToString(),
            Text = createCommentDto.Comment,
            ArticleId =  createCommentDto.ArticleId
        };
        var hasProfanity = await profanityClient.FilterComment(comment.Text);
        if (hasProfanity)
        {
            throw new Exception("Comment contains forbidden words");
        }
        await commentsRepo.SaveComment(comment,  createCommentDto.UserId);
    }

    public async Task<CommentsListDto> GetComments(ArticleDto articleDto)
    {
        using var activity = MonitorService.MonitorService.ActivitySource.StartActivity();
        
        MonitorService.MonitorService.Log.Debug("Entered GetComments in CommentsService");

        
        var comments = await commentsRepo.GetComments(articleDto.ArticleId);
        var commentsListDto = new CommentsListDto
        {
            Comments = comments.Select(c => CommentsListDto.FromEntity(c)).ToList()
        };

        return commentsListDto;
        
    }
}