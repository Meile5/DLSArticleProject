using System;
using System.Collections.Generic;

namespace DraftService.Models;

public partial class DraftStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Draft> Drafts { get; set; } = new List<Draft>();
}
