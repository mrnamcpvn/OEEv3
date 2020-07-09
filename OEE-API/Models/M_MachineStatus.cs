using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_MachineStatus
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int status_id { get; set; }
        public string status_name { get; set; }
        public string status_note { get; set; }
    }
}