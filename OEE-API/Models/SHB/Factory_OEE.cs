namespace OEE_API.Models.SHB
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Factory_OEE
    {
        public int id { get; set; }

        public DateTime? Time { get; set; }

        [StringLength(50)]
        public string Factory { get; set; }

        public int? Availability { get; set; }
    }
}
