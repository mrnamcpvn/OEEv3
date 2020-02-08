using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SYF
{
    public partial class Building_OEE
    {
        public int id { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(50)]
        public string Factory { get; set; }

        [StringLength(50)]
        public string Building { get; set; }

        public int? Availability { get; set; }
    }
}
