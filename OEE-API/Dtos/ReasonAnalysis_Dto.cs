namespace OEE_API.Dtos
{
    public class ReasonAnalysis_Dto
    {
        public int? id {get;set;}
        public string reason_1 {get;set;}
        public string reason_2 {get;set;}
        public decimal? duration {get;set;}

        public string machine_type {get;set;}
    }
}