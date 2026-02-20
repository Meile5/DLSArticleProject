using System;
using System.Collections.Generic;

namespace CommentService.Entities;

public partial class CommentUser
{
    public string Id { get; set; } = null!;

    public string CommentId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual Comment Comment { get; set; } = null!;
}
