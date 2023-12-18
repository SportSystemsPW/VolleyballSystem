using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.DTO.TrainingOrganizer
{
    public class TrainingOrganizerResponse<T>
    {
        public T? Content { get; set; }
        public List<string>? Messages { get; set; }
    }
}
