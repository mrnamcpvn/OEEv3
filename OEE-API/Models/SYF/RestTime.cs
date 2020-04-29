using System;

namespace OEE_API.Models.SYF
{
public class RestTime 
{
    public string factoryid {get;set;}

    public int? shift_id {get;set;}

    public string building_id {get;set;}
    public DateTime? start_time {get;set;}
    public DateTime? end_time {get;set;}
    public string shift_notes {get;set;}
}
}
