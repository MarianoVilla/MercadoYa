using GeneralUtil.Model;
using MercadoYa.Model.CustomDataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace MercadoYa.Model.Concrete
{
    public class StoreDaySchedule
    {
        public string MightExtendIf { get; set; }
        public string MightShrinkIf { get; set; }
        public IEnumerable<DaySchedule> DailyIntervals { get; set; }
    }
}
