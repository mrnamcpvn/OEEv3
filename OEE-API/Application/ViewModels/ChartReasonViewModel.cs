using System;

namespace OEE_API.Application.ViewModels
{
    public class ChartReason 
    {
        public int? id {get;set;}
        public string title {get;set;}
         public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string factory_id {get;set;}
        public string building_id {get;set;}
        public string machine_id {get;set;}
         public int? diffTime {get;set;}
         public string shift_id {get;set;}
         public DateTime? shift_date{ get;set;}
         public string reason_1 {get;set;}
        public string reason_2 {get;set;}
        public string reason_note {get;set;}

    }
}