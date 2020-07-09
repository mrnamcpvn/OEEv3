using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_Shift
    {
        [Key]
        public int shift_id { get; set; }
        public string shift_name { get; set; }
        public string shift_notes { get; set; }
    }
}