using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Models.SHW_SHD
{
    public class DowntimeDetail
    {
       public int id { get; set; }
        public int? actionTime_id { get; set; }
        [StringLength(10)]
        public string factory_id { get; set; }
        [StringLength(10)]
        public string building_id { get; set; }
        [StringLength(10)]
        public string machine_id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? shiftdate { get; set; }
        [StringLength(1)]
        public string shift_id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? start_time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? end_time { get; set; }
        public int? duration { get; set; }
        public int? reason_id { get; set; }
        [StringLength(1)]
        public string remark { get; set; }
        [StringLength(100)]
        public string notes { get; set; }
    }
}