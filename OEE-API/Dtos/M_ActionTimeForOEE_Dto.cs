using System;

namespace OEE_API.Dtos
{
    public class M_ActionTimeForOEE_Dto
    {
        public decimal id { get; set; }
        public string factory_id { get; set; }
        public string machine_id { get; set; }
        public string building_id { get; set; }
        public DateTime? shift_date { get; set; }
        public DateTime? date { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public decimal duration_sec { get; set; }
        public int machine_status_id { get; set; }
        public int shift_id { get; set; }
        public int time_split_group_1 { get; set; }
        public int time_group_for_oee { get; set; }
        public bool is_work_time { get; set; }
    }
}