namespace DraftService.Models;

public class DraftDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string AuthorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DraftStatusEnum Status { get; set; }
}

public enum DraftStatusEnum
{
    Draft,
    PendingReview,
    Approved,
    Rejected
}