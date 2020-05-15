using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OEE_API.Utilities
{
    public class PageListUtility<T> where T : class
    {
        public PagedResultBase Pagination { get; set; }
        public List<T> Result { get; set; }
        public List<T> ResultC { get; set; }
        public string machineName {get;set;}

        public PageListUtility(List<T> itemsC, List<T> items, string machine, int count, int pageNumber, int pageSize, int skip)
        {
            Result = items;
            ResultC = itemsC;
            machineName = machine;
            Pagination = PagedResultBase.PageList(count, pageNumber, pageSize, skip);
        }

        /// <summary>
        /// Phân trang theo tiện ích Update 10/01/2019
        /// </summary>
        /// <param name="source">Danh sách truyền vào</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng dòng trên một trang</param>
        /// <returns> Một đối tượng PageListUtility theo kiểu data truyền vào </returns>
        public static async Task<PageListUtility<T>> PageListAsync(IQueryable<T> source, string machineName, int pageNumber, int pageSize = 10)
        {
            var count = await source.CountAsync();
            int skip = (pageNumber - 1) * pageSize;
            var items = await source.Skip(skip).Take(pageSize).ToListAsync();
        var itemsC = await source.ToListAsync();
            return new PageListUtility<T>(itemsC, items, machineName, count, pageNumber, pageSize, skip);
        }

        public static async Task<PageListUtility<T>> PageListAsyncChartReason(IQueryable<T> source, IQueryable<T> sourceT, string machineName ,int pageNumber, int pageSize = 10)
        {
            var count = await sourceT.CountAsync();
            int skip = (pageNumber - 1) * pageSize;
            var items = await sourceT.Skip(skip).Take(pageSize).ToListAsync();
         var itemsC = await source.ToListAsync();
            return new PageListUtility<T>(itemsC, items, machineName, count, pageNumber, pageSize, skip);
        }
        public class PagedResultBase
        {
            public int CurrentPage { get; set; }
            public int TotalPage { get; set; }
            public int PageSize { get; set; }
            public int TotalCount { get; set; }
            public int Skip { get; set; }

            public PagedResultBase(int count, int pageNumber, int pageSize, int skip)
            {
                TotalCount = count;
                TotalPage = (int)Math.Ceiling(TotalCount / (double)pageSize);
                CurrentPage = pageNumber < 1 ? 1 : (pageNumber > TotalPage ? TotalPage : pageNumber);
                PageSize = pageSize;
                Skip = skip;
            }
            public static PagedResultBase PageList(int count, int pageNumber, int pageSize, int skip)
            {
                return new PagedResultBase(count, pageNumber, pageSize, skip);
            }
        }
    }
}
