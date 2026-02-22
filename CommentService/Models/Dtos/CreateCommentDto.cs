namespace CommentService.Models.Dtos;

public class CreateCommentDto
{
    public string Comment { get; set; }
    public string ArticleId { get; set; }
    public string UserId { get; set; }
}