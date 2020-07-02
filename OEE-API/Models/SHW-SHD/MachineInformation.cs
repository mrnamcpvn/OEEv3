using System;
using System.ComponentModel.DataAnnotations;
namespace OEE_API.Models.SHW_SHD
{
    public class MachineInformation
    {
        public int machine_id { get; set; }
        public string machine_model { get; set; }
        public string machine_type { get; set; }
        public string factory_id { get; set; }
        public string building_id { get; set; }
        public string line_id { get; set; }
        public string alias_id { get; set; }
        public string conversion_flag { get; set; }
    }
    }