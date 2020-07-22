using System.ComponentModel.DataAnnotations;

namespace OEE_API.Models
{
    public class M_DowntimeRecord
    {
        [Key]
        public int action_time_id { get; set; }

        public int downtime_reason_id { get; set; }

        public string remark { get; set; }
    }
}