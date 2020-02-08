using System;
using System.Collections.Generic;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Interfaces
{
    public interface IDowntimeReasonsService
    {
         List<ActionTime> GetDuration(string factory, string machine, string shift, string date);
    }
}