namespace OEE_API.Dtos
{
    public class M_DowntimeRecord_Dto
    {
        public int action_time_id { get; set; }

        public int downtime_reason_id { get; set; }

        public string remark { get; set; }

        public string note { get; set; }
    }
}