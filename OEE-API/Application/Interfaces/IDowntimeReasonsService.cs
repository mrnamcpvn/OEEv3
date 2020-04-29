using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API.Application.ViewModels;
using OEE_API.Models.SHW_SHD;
using OEE_API.Utilities;

namespace OEE_API.Application.Interfaces
{
    public interface IDowntimeReasonsService
    {
         Task<PageListUtility<ChartReason>> GetDuration(string factory, string building, string machine, string shift, string date, int page = 1);

          Task<List<string>> GetDowntimeReasonDetail(string reason_1);
          Task<bool> AddDowntimeReason(ChartReason chartReason);


    }
}