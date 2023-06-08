using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Infrastructure.Database.Models
{
    public class MessageTemplate
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public int TrainerId { get; set; }
        public virtual Trainer Trainer { get; set; } = null!;
    }
}
