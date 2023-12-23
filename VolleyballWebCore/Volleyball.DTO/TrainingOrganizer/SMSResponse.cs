﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.DTO.TrainingOrganizer
{
    public class SMSResponse
    {
        public string Phone { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class AttendanceChangedResponse
    {
        public int TrainingId { get; set; }
        public int AttendanceCountDelta { get; set; }
    }
}
