using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_RowIndex
    {
        
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int row_index { get; set; }
    }
}