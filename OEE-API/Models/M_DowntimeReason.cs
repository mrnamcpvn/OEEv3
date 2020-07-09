using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Models
{
    public class M_DowntimeReason
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string reason_type { get; set; }

        public string reason { get; set; }
    }
}