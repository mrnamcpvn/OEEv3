using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_Users
    {
        [Key]
        public string account { get; set; }

        public string password { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public string update_by { get; set; }

        public DateTime? update_time { get; set; }
    }
}