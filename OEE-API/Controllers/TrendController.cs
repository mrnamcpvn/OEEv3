using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OEE_API.Application.Interfaces;
using OEE_API.Data.Interfaces;
using OEE_API.Models.SHW_SHD;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace OEE_API.Controllers
{
    public class TrendController : ApiController
    {
        private readonly ITrendService _trendService;
        public TrendController(ITrendService trendService)
        {
            _trendService = trendService;
        }

        [HttpGet("GetAvailabilityOfTrend")]
        public async Task<IActionResult> GetListAvailabilityAsync(string factory, string building, string shift, string typeTime, string numberTime)
        {
            var number = numberTime != string.Empty ? Convert.ToInt32(numberTime) : 1;

            // Get availability by week
            if (typeTime == "week")
            {
                var data = await _trendService.GetTrendByWeek(factory, building, shift, number);
                return Ok(data);
            }
            // Get availability by month
            if (typeTime == "month")
            {
                var data = await _trendService.GetTrendByMonth(factory, building, shift, number);
                return Ok(data);
            }
            // Get availability by year
            if (typeTime == "year")
            {
                var data = await _trendService.GetTrendByYear(factory, building, shift);
                return Ok(data);
            }

            return NoContent();
        }

        // [HttpGet("Export")]
        // public IActionResult ExportExcel()
        // {

        //     ExcelPackage excelfilecontent = new ExcelPackage();

        //     var worksheet = excelfilecontent.Workbook.Worksheets.Add("Sheet1");
        //     worksheet.Cells[2, 1].Value = "Hello";
        //     worksheet.Cells[2, 1].Style.Font.Bold = true;
        //     worksheet.Cells[2, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        //     worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        //     worksheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //     worksheet.Cells[2, 1].Style.Font.Color.SetColor(System.Drawing.Color.Black);
        //     worksheet.Cells[2, 1].Style.WrapText = true;
        //     worksheet.Cells[2, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);

        //     byte[] dataExcel;
        //      dataExcel = excelfilecontent.GetAsByteArray();


        //     return File(dataExcel, "application/xlsx", "ExportHistoryInventory_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".xlsx");

        // }
    }
}
