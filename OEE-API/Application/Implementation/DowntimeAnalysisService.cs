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
using OEE_API.Application.Interfaces.SHW_SHD;
using System.Linq;

namespace OEE_API.Application.Implementation
{
    public class DowntimeAnalysisService : IDowntimeAnalysisService
    {
        ServiceSHW_SHD.IDownTimeDetailService _DownTimeServiceSHW_SHD;


        ServiceSHB.ICell_OEEService _Cell_OEEServiceSHB;
        ServiceSYF.ICell_OEEService _Cell_OEEServiceSYF;


        private readonly MapperConfiguration _configureMapper;
        public DowntimeAnalysisService(
           ServiceSHW_SHD.IDownTimeDetailService DownTimeServiceSHW_SHD,
           ServiceSHB.ICell_OEEService Cell_OEEServiceSHB,
           ServiceSYF.ICell_OEEService Cell_OEEServiceSYF,

           MapperConfiguration configureMapper)
        {
            _DownTimeServiceSHW_SHD = DownTimeServiceSHW_SHD;
            _Cell_OEEServiceSHB = Cell_OEEServiceSHB;
            _Cell_OEEServiceSYF = Cell_OEEServiceSYF;
            _configureMapper = configureMapper;
        }

        public async Task<Object> GetDownTimeAnalysis(string factory, string building, string machine, string shift, string date)
        {
          //  var data = await _ActionTimeServiceSHW_SHD.GetDuration(factory, building, machine, shift, date);
          List<ReasonAnalysis> result = new List<ReasonAnalysis>();
            var SHW_SHD_analysis = await _DownTimeServiceSHW_SHD.GetDownTimeAnalysis(factory,building,machine,shift,date);
            if(SHW_SHD_analysis.Count > 0)
            {
              var list = SHW_SHD_analysis.GroupBy( x=> x.reason_1).Select(grp => grp.ToList()).ToList();
      
              foreach(var item in list)
              {
                ReasonAnalysis rea_2 = new ReasonAnalysis();
                var num = item.Sum(x=> x.duration);
                rea_2.reason_1 = item[0].reason_1;
                rea_2.duration = num.Value;
                result.Add(rea_2);
              }
            }
           return new {
               resA = SHW_SHD_analysis.Take(10),
               resB = result
           };
    
        }

    }
}