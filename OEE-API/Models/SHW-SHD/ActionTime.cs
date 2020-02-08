using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SHW_SHD
{
    public partial class ActionTime
    {
        public int id { get; set; }

        [StringLength(150)]
        public string factory_id { get; set; }
        [StringLength(50)]
        public string machine_id { get; set; }

        [StringLength(150)]
        public string building_id { get; set; }

        public DateTime? shiftdate { get; set; }

        [StringLength(150)]
        public DateTime? date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }

        [StringLength(150)]
        public string duration { get; set; }
        [StringLength(150)]
        public string status_id { get; set; }
        [StringLength(150)]
        public string status { get; set; }
        [StringLength(150)]
        public string shift { get; set; }
    }
}