using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SYF
{
    public partial class Cell_OEE
    {
        public int id { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(50)]
        public string Factory { get; set; }

        [StringLength(50)]
        public string Building { get; set; }

        [StringLength(50)]
        public string Machine { get; set; }

        public int? Downtime { get; set; }

        public int? Availability { get; set; }
    }
}
