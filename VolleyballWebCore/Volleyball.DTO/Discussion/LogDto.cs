using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volleyball.DTO.Teams;

namespace Volleyball.DTO.Discussion
{
    public class LogDto
    {
        public int Id { get; set; }

        public string? Link { get; set; }

        public string Description { get; set; } = null!;

        public DateTime Date { get; set; }

        public PlayerSummaryDto? User { get; set; }

        public static explicit operator LogDto(Volleyball.Infrastructure.Database.Models.PersonalLog personalLog)
        {
            return new LogDto
            {
                Id = personalLog.Id,
                Link = personalLog.Log.Link,
                Description = personalLog.Log.Description,
                Date = personalLog.Log.Date,
                User = (PlayerSummaryDto?)personalLog.User
            };
        }

        public static explicit operator LogDto(Volleyball.Infrastructure.Database.Models.Log log)
        {
            return new LogDto
            {
                Id = log.Id,
                Link = log.Link,
                Description = log.Description,
                Date = log.Date
            };
        }
    }
}
