using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.CoreObject.ViewModels
{
    public class PagedList<T> : List<T>
    {
        /// <summary>
        ///
        /// </summary>
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        /// <param name="count"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// Method to generate paginated list from IQueryable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, string? sortColumn = null, string? sortDirection = null)
        {
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                var column = typeof(T).GetProperty(sortColumn)?.GetType();
                if (column != null)
                {
                    if (!string.IsNullOrEmpty(sortDirection) && sortDirection.ToLower() == "desc")
                    {
                        source = source.OrderByDescending(x => column);
                    }
                    else
                    {
                        source = source.OrderBy(x => column);
                    }
                }

            }
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }

}
