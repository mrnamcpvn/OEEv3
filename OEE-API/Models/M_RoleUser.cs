using System;
using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_RoleUser
    {
        [Key]
        public long id { get; set; }
        public string user_account { get; set; }
        public string role_unique { get; set; }
        public string create_by { get; set; }
        public DateTime? create_time { get; set; }
    }
}