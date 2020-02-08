using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SHW_SHD
{
    public partial class SHW_OEE_History
    {
        public int id { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(50)]
        public string Factory { get; set; }

        [StringLength(50)]
        public string Building { get; set; }

        [StringLength(50)]
        public string Machine { get; set; }

        [StringLength(50)]
        public string Downtime { get; set; }

        public int? Availability { get; set; }
    }
}
