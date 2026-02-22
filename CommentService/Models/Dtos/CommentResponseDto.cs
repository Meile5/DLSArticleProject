namespace CommentService.Models.Dtos;

public class CommentResponseDto
{
    public string CommentId { get; set; }
    public string Comment { get; set; }
    public string ArticleId { get; set; }
    public string UserId { get; set; }
}