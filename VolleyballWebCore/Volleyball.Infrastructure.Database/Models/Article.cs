using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Volleyball.Infrastructure.Database.Models
{
    public partial class Article
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public string Title { get; set; } = null!;

        public bool? IsActive { get; set; }

        public byte[] Image { get; set; } = null!;

        public virtual User Author { get; set; } = null!;
    }
}
