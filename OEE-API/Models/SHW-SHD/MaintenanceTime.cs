using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Models.SHW_SHD
{
    public class MaintenanceTime
    {
        [Key]
        public int id { get; set; }
        [StringLength(50)]
        public string machine_id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? start_time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? end_time { get; set; }
    }
}
