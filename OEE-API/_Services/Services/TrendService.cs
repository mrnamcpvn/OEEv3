using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API._Services.Interfaces;
using OEE_API.Helpers;
using OEE_API.ViewModels;

namespace OEE_API._Services.Services
{
    public class TrendService : ITrendService
    {
        public async Task<object> DataCharTrend(TrendParamModel param)
        {
             if(param.count >=4) {
                var numberWeek = param.numberTime != string.Empty ? Convert.ToInt32(param.numberTime) : 1;
             // take out a week to follow 'numberWeek
                var week = Util.ListWeekOfYear().Find(x => x.weekNum == numberWeek);

                DateTime dateStart = week.weekStart.Value;
                DateTime dateEnd = week.weekFinish.Value;

                // Take out list day of week
                List<DateTime> rangerDate = Util.GetRangerDates(dateStart, dateEnd);
                List<ChartTrendViewModel> dataChart = new List<ChartTrendViewModel>();

                // Reformat the list of dates to display outside
                List<string> listTime = rangerDate.ConvertAll(x => x.ToString("MM/dd"));
                var result = new {
                    listTime
                };
                return result;
            } 
            return null;
            // throw new System.NotImplementedException();
        }
    }
}