using System;
using System.Collections.Generic;

namespace Arbiter.API.Data.Models;

public partial class ForumCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ForumTopic> ForumTopics { get; set; } = new List<ForumTopic>();
}
