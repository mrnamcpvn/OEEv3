using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Models.SHW_SHD
{
    public partial class ActionTime
    {[Key]
        public int id { get; set; }
        [StringLength(50)]
        public string factory_id { get; set; }
        [StringLength(50)]
        public string machine_id { get; set; }
        [StringLength(50)]
        public string building_id { get; set; }
        [Column(TypeName = "date")]
        public DateTime? shiftdate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? date { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? start_time { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? end_time { get; set; }
        [StringLength(50)]
        public string duration { get; set; }
        [StringLength(50)]
        public string status_id { get; set; }
        [StringLength(50)]
        public string status { get; set; }
        [StringLength(50)]
        public string shift { get; set; }
    }
}