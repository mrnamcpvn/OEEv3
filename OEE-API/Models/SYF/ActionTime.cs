using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SYF
{
    public class ActionTime
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Factory_ID { get; set; }

        [StringLength(50)]
        public string Machine_ID { get; set; }

        public DateTime Shiftdate { get; set; }
        public DateTime Start_Time { get; set; }
        public DateTime End_Time { get; set; }
        public int Duration { get; set; }
        public int Shift { get; set; }
        public int Status_ID { get; set; }
        [StringLength(10)]
        public string Status { get; set; }

    }
}