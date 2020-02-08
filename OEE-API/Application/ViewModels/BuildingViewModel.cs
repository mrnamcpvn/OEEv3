using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Application.ViewModels
{
    public class BuildingViewModel
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