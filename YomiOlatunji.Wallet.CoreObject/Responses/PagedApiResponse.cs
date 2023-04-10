using YomiOlatunji.Wallet.CoreObject.Enumerables;
using YomiOlatunji.Wallet.CoreObject.ViewModels;

namespace YomiOlatunji.Wallet.CoreObject.Responses
{
    public class PagedApiResponse<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<T> data { get; set; }
        public string code { get; set; }
        public string message { get; set; }

        public static PagedApiResponse<T> Load(PagedList<T> data)
        {
            return new PagedApiResponse<T>()
            {
                CurrentPage = data.CurrentPage,
                PageSize = data.PageSize,
                TotalCount = data.TotalCount,
                TotalPages = data.TotalPages,
                data = data.ToList(),
            };
        }

        public static PagedApiResponse<T> Success(PagedList<T> _data, string _message = "Successful")
        {
            return new PagedApiResponse<T>
            {
                code = ResponseCodes.Success.code,
                message = _message,
                data = _data,
                CurrentPage = _data.CurrentPage,
                PageSize = _data.PageSize,
                TotalCount = _data.TotalCount,
                TotalPages = _data.TotalPages,
            };
        }

        public static PagedApiResponse<T> Failed(PagedList<T> _data, string _message = "Failed")
        {
            return new PagedApiResponse<T> { code = ResponseCodes.Failed.code, message = _message, data = _data };
        }

        public static PagedApiResponse<T> NoRecordFound(string _message = "No record found")
        {
            return new PagedApiResponse<T> { code = ResponseCodes.NotFound.code, message = _message };
        }

        public PagedApiResponse()
        {
        }

        public static PagedApiResponse<T> Empty()
        {
            return new PagedApiResponse<T>()
            {
                data = new List<T>()
            };
        }
    }
}