namespace OEE_API.ViewModels
{
    public class DownTimeReasonParamModel
    {
        public string factory {get;set;}
        public string building {get;set;}
        public string machine_type {get;set;}
        public string machine_id {get;set;}
        public string shift_id {get;set;}
        public string date {get;set;}
    }
}