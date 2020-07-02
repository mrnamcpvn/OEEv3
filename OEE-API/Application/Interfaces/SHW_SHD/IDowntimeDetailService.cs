using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API.Application.ViewModels;
using OEE_API.Models.SHW_SHD;
using OEE_API.Utilities;

namespace OEE_API.Application.Interfaces.SHW_SHD
{
    public interface IDownTimeDetailService
    {
            Task<List<ReasonAnalysis>> GetDownTimeAnalysis(string factory, string building, string machine_type, string machine, string shift, string date);

    }
}