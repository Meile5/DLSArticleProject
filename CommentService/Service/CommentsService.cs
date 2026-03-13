using CommentService.Clients;
using CommentService.Entities;
using CommentService.Models.Dtos;
using CommentService.Repositories;
using MonitorService;
using Serilog;

namespace CommentService.Service;

public class CommentsService(CommentsRepo commentsRepo, IProfanityClient profanityClient, CacheService cacheService)
{
    public async Task SaveComment(CreateCommentDto createCommentDto)
    {
        //for OpenTelemetry/Zipkin
        using var activity = Monitoring.ActivitySource.StartActivity("Entered SaveComment in CommentsService");
        
        //for Serilog debugging
        Log.Logger.Debug("Entered SaveComment in CommentsService");

        
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

        var commentDto = new CommentResponseDto
        {
            CommentId = comment.CommentId,
            ArticleId = comment.ArticleId,
            Comment = comment.Text,

        };
        await commentsRepo.SaveComment(comment,  createCommentDto.UserId);
        var commentCacheList = await cacheService.GetAsync<List<CommentResponseDto>>(createCommentDto.ArticleId);
        
        if (commentCacheList != null)
        {
            commentCacheList.Add(commentDto);
            await cacheService.SetAsync(createCommentDto.ArticleId, commentCacheList );
            
        }else
        {
            var commentlist = new List<CommentResponseDto>();
            commentlist.Add(commentDto);

            await cacheService.SetAsync(createCommentDto.ArticleId, commentlist);
            
        }
    }

    public async Task<CommentsListDto> GetComments(ArticleDto articleDto)
    {
        using var activity = Monitoring.ActivitySource.StartActivity("Entered GetComments in CommentsService");
        
        Log.Logger.Debug("Entered GetComments in CommentsService");
        
        var commentList = await cacheService.GetAsync<List<CommentResponseDto>>(articleDto.ArticleId);

        var commentsListDto = new CommentsListDto();

        if (commentList != null)
        {
            commentsListDto.Comments = commentList;
            return commentsListDto;
        }
        
        var comments = await commentsRepo.GetComments(articleDto.ArticleId);
        commentsListDto.Comments = comments.Select(c => CommentsListDto.FromEntity(c)).ToList();
        await cacheService.SetAsync(articleDto.ArticleId, commentsListDto.Comments);

        return commentsListDto;
        
    }
}