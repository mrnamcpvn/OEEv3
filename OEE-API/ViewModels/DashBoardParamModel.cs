using System;

namespace OEE_API.ViewModels
{
    public class DashBoardParamModel
    {
        public string factory {get;set;}
        public string building {get;set;}
        public string machine_id {get;set;}
        public string shift_id {get;set;}
        public string month {get;set;}
        public string date {get;set;}
        public string dateTo {get;set;}
    }
}