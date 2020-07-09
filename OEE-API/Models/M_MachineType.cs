using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_MachineType
    {
        [Key]
        public int id { get; set; }
        public string machine_type_name { get; set; }
        public string note { get; set; }
    }
}