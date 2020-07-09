using System;

namespace OEE_API.Dtos
{
    public class M_Roles_Dto
    {
        public string role_unique { get; set; }
        public string role_name { get; set; }
        public string role_note { get; set; }
        public double role_sequence { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }
    }
}