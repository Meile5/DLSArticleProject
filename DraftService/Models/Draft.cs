using System;
using System.Collections.Generic;

namespace DraftService.Models;

public partial class Draft
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string AuthorId { get; set; } = null!;

    public int StatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual DraftStatus Status { get; set; } = null!;
}
