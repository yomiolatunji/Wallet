using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBSC.Wallet.CoreObject.ViewModels
{
    public class PagedRequest
    {
        private const int maxPageSize = 50;
        private const int minPageNumber = 1;
        public string SearchQuery { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

        private int _pageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value < minPageNumber) ? minPageNumber : value;
        }

        private int _pageSize = 20;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
