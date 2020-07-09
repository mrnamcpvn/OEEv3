using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_MachineInformation
    {
        public string factory_id { get; set; }
        public string building_id { get; set; }
        [Key]
        public string machine_id { get; set; }
        public string machine_name { get; set; }
        public string machine_model { get; set; }
        public int machine_type { get; set; }
        public string line_id { get; set; }
        public bool is_active { get; set; }
    }
}