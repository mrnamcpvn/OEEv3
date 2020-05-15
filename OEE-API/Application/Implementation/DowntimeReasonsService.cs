using ServiceSHW_SHD = OEE_API.Application.Interfaces.SHW_SHD;
using ServiceSHB = OEE_API.Application.Interfaces.SHB;
using ServiceSYF = OEE_API.Application.Interfaces.SYF;
using OEE_API.Utilities;
using OEE_API.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Application.Interfaces;
using OEE_API.Models.SHW_SHD;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace OEE_API.Application.Implementation
{
    public class DowntimeReasonsService : IDowntimeReasonsService
    {
        ServiceSHW_SHD.IActionTimeService _ActionTimeServiceSHW_SHD;


        ServiceSHB.ICell_OEEService _Cell_OEEServiceSHB;
        ServiceSYF.ICell_OEEService _Cell_OEEServiceSYF;


        private readonly MapperConfiguration _configureMapper;
        public DowntimeReasonsService(
           ServiceSHW_SHD.IActionTimeService ActionTimeServiceSHW_SHD,
           ServiceSHB.ICell_OEEService Cell_OEEServiceSHB,
           ServiceSYF.ICell_OEEService Cell_OEEServiceSYF,

           MapperConfiguration configureMapper)
        {
            _ActionTimeServiceSHW_SHD = ActionTimeServiceSHW_SHD;
            _Cell_OEEServiceSHB = Cell_OEEServiceSHB;
            _Cell_OEEServiceSYF = Cell_OEEServiceSYF;
            _configureMapper = configureMapper;
        }

        public async Task<PageListUtility<ChartReason>> GetDuration(string factory, string building, string machine, string shift, string date, int page = 1)
        {
            if(factory == "SHD" || factory == "SHW")
            {
            var data = await _ActionTimeServiceSHW_SHD.GetDuration(factory, building, machine, shift, date, page);
             return data;
            }
            else return null;
        }

        public async Task<List<string>> GetDowntimeReasonDetail(string reason_1)
        {
            var data = await _ActionTimeServiceSHW_SHD.GetDowntimeReasonDetail(reason_1);

            return data;
        }
        public async Task<bool> AddDowntimeReason(ChartReason chartReason)
        {
            var data = await _ActionTimeServiceSHW_SHD.AddDowntimeReason(chartReason);

            return data;
        }
           public async Task<ReasonDetail> GetReasons(int item)
        {
             var data = await _ActionTimeServiceSHW_SHD.GetReasons(item);

            return data;
        }
    }
}