using CommentService.Entities;

namespace CommentService.Models.Dtos;

public class CommentsListDto
{
    public List<CommentResponseDto> Comments { get; set; }
    
    
    // Converting Entity into Dto
    public static CommentResponseDto FromEntity(Comment comment)
    {
        return new CommentResponseDto
        {
            CommentId = comment.CommentId,
            ArticleId =  comment.ArticleId,
            Comment = comment.Text,
            UserId = comment.CommentUsers.FirstOrDefault()?.UserId ?? string.Empty
        };
    }
}