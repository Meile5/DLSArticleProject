using System;
using System.Collections.Generic;

namespace CommentService.Entities;

public partial class Comment
{
    public string CommentId { get; set; } = null!;

    public string Text { get; set; } = null!;

    public string ArticleId { get; set; } = null!;

    public virtual ICollection<CommentUser> CommentUsers { get; set; } = new List<CommentUser>();
}
