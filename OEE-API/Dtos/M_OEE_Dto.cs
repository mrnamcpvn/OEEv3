using System;

namespace OEE_API.Dtos
{
    public class M_OEE_Dto
    {
        public string factory_id { get; set; }
        public string building_id { get; set; }
        public string machine_id { get; set; }
        public DateTime? shift_date { get; set; }
        public DateTime? date { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public decimal run_time_sec { get; set; }
        public decimal work_time_sec { get; set; }
        public decimal maintenance_run_time_sec { get; set; }
        public decimal maintenance_work_time_sec { get; set; }
        public decimal oee_rate { get; set; }
        public int time_group_for_oee { get; set; }
        public int shift_id { get; set; }
        public int shift_year { get; set; }
        public int shift_month { get; set; }
        public int shift_day { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
    }
}