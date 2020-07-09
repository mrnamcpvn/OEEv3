using System;

namespace OEE_API.Dtos
{
    public class M_ShiftTimeConfig_Dto
    {
        public string factory_id { get; set; }
        public string building_id { get; set; }
        public int shift_id { get; set; }
        public TimeSpan start_time { get; set; }
        public TimeSpan end_time { get; set; }
        public decimal diff_time_sec { get; set; }
        public int time_split_group_1 { get; set; }
        public int time_split_group_2 { get; set; }
        public int time_group_for_oee { get; set; }
        public bool is_work_time { get; set; }
        public int add_work_day { get; set; }
    }
}