using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Models.SHW_SHD
{
    public class M_Users
    {

        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string update_by { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime update_time { get; set; }
    }
}