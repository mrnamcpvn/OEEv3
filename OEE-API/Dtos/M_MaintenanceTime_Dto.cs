using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Dtos
{
    public class M_MaintenanceTime_Dto
    {
        public decimal id { get; set; }
        public string factory_id { get; set; }

        public string building_id { get; set; }
        public string machine_id { get; set; }

        public DateTime maintenance_date { get; set; }

        public TimeSpan start_time { get; set; }

        public TimeSpan end_time { get; set; }

        public string update_by { get; set; }

        public DateTime update_time { get; set; }

        public bool is_calculate { get; set; }
    }
}