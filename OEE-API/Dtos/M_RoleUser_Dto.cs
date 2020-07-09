using System;

namespace OEE_API.Dtos
{
    public class M_RoleUser_Dto
    {
        public long id { get; set; }
        public string user_account { get; set; }
        public string role_unique { get; set; }
        public string create_by { get; set; }
        public DateTime? create_time { get; set; }
    }
}