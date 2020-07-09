using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Models
{
    public class M_OEE_MM
    {
        [Key]
        [Column(Order = 0)]
        public string factory_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string building_id { get; set; }

        [Key]
        [Column(Order = 2)]
        public string machine_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? shift_date { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime? date { get; set; }

        public TimeSpan start_time { get; set; }

        public TimeSpan end_time { get; set; }

        [Column(TypeName = "numeric")]
        public decimal run_time_sec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal work_time_sec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal maintenance_run_time_sec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal maintenance_work_time_sec { get; set; }

        [Column(TypeName = "numeric")]
        public decimal oee_rate { get; set; }

        [Key]
        [Column(Order = 4)]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
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