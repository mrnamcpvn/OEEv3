using System;
using System.Collections.Generic;
using System.Linq;
using OEE_API.ViewModels;

namespace OEE_API.Helpers
{
    public static class Util
    {
         public static List<WeekViewModel> ListWeekOfYear()
        {
            var jan = new DateTime(DateTime.Today.Year, 1, 1);

            var startOfFirstWeek = jan.AddDays(1 - (int)(jan.DayOfWeek));
            var weeks =
                Enumerable
                    .Range(0, 54)
                    .Select(i => new
                    {
                        weekStart = startOfFirstWeek.AddDays(i * 7)
                    })
                    .TakeWhile(x => x.weekStart.Year <= jan.Year)
                    .Select(x => new
                    {
                        x.weekStart,
                        weekFinish = x.weekStart.AddDays(5)
                    })
                    .SkipWhile(x => x.weekFinish < jan.AddDays(1))
                    .Select((x, i) => new WeekViewModel
                    {
                        weekStart = x.weekStart,
                        weekFinish = x.weekFinish,
                        weekNum = i + 1
                    });

            return weeks.ToList();
        }

        // Get list day from day start and day end 
        public static List<DateTime> GetRangerDates(DateTime start, DateTime end)
        {
            List<DateTime> dates = new List<DateTime>();

            while (start <= end)
            {
                dates.Add(start);
                start = start.AddDays(1);
            }

            return dates;
        }

        // List month of year
        public static List<MonthViewModel> ListMonth()
        {
            List<MonthViewModel> listMonth = new List<MonthViewModel>();
            listMonth.Add(new MonthViewModel { NumberMonth = 1, NameMonth = "January" });
            listMonth.Add(new MonthViewModel { NumberMonth = 2, NameMonth = "February" });
            listMonth.Add(new MonthViewModel { NumberMonth = 3, NameMonth = "March" });
            listMonth.Add(new MonthViewModel { NumberMonth = 4, NameMonth = "April" });
            listMonth.Add(new MonthViewModel { NumberMonth = 5, NameMonth = "May" });

            listMonth.Add(new MonthViewModel { NumberMonth = 6, NameMonth = "June" });
            listMonth.Add(new MonthViewModel { NumberMonth = 7, NameMonth = "July" });
            listMonth.Add(new MonthViewModel { NumberMonth = 8, NameMonth = "August" });
            listMonth.Add(new MonthViewModel { NumberMonth = 9, NameMonth = "September" });
            listMonth.Add(new MonthViewModel { NumberMonth = 10, NameMonth = "October" });

            listMonth.Add(new MonthViewModel { NumberMonth = 11, NameMonth = "November" });
            listMonth.Add(new MonthViewModel { NumberMonth = 12, NameMonth = "December" });

            return listMonth;
        }
    }
}