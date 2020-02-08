using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models.SYF
{
    public partial class Factory_OEE
    {
        public int id { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(50)]
        public string Factory { get; set; }

        public int? Availability { get; set; }
    }
}
