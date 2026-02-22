namespace CommentService.Clients;

public interface IProfanityClient
{
    Task<bool> FilterComment(string text);
}