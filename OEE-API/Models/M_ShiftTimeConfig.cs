using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEE_API.Models
{
    public class M_ShiftTimeConfig
    {
        [Key]
        [Column(Order = 0)]
        public string factory_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string building_id { get; set; }

        [Key]
        [Column(Order = 2)]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int shift_id { get; set; }

        [Key]
        [Column(Order = 3)]
        public TimeSpan start_time { get; set; }

        [Key]
        [Column(Order = 4)]
        public TimeSpan end_time { get; set; }

        [Column(TypeName = "numeric")]
        public decimal diff_time_sec { get; set; }

        public int time_split_group_1 { get; set; }

        public int time_split_group_2 { get; set; }

        public int time_group_for_oee { get; set; }

        public bool is_work_time { get; set; }

        public int add_work_day { get; set; }
    }
}