using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SHB
{
    public class OeeReport_test
    {
       
        public int id { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(50)]
        public string Factory { get; set; }
        [StringLength(50)]
        public string Building { get; set; }
        [StringLength(50)]
        public string Machine { get; set; }
         public DateTime? Shiftdate { get; set; }

        
        [StringLength(50)]
        public string Shift_ID { get; set; }
        public int? Availability { get; set; }
           [StringLength(1000)]
        public string Remark { get; set; }
    }
}