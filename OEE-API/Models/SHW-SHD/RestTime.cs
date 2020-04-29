using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SHW_SHD
{
    public class RestTime
    {
        [Key]
        [StringLength(50)]
        public string factory_id { get; set; }
        [Key]
        [StringLength(50)]
        public string shift_id { get; set; }
        [Key]
        [StringLength(50)]
        public string building_id { get; set; }
        public TimeSpan? start_time { get; set; }
        public TimeSpan? end_time { get; set; }
        [StringLength(50)]
        public string shift_notes { get; set; }
    }
}
