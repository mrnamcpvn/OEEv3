using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_Factory
    {
        [Key]
        public string factory_id { get; set; }
        public string factory_name { get; set; }
        public string customer_name { get; set; }
        public string location { get; set; }
    }
}