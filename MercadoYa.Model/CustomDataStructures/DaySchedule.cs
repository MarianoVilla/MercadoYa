using GeneralUtil.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.CustomDataStructures
{
    public class DaySchedule
    {
        public DayOfWeek Day { get; set; }
        public Interval<DateTime> OpenIntervals { get; set; }
    }
}
