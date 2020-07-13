using System;

namespace OEE_API.ViewModels
{
    public class WeekViewModel
    {
        public DateTime? weekStart { get; set; }
        public DateTime? weekFinish { get; set; }
        public int weekNum { get; set; }
    }
}