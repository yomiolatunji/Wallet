using YomiOlatunji.Wallet.CoreObject.Enumerables;

namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class ApiResponse<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="_message"></param>
        /// <returns></returns>
        public static ApiResponse<T> Success(T _data, string _message = "Successful")
        {
            return new ApiResponse<T> { Code = ResponseCodes.Success.code, Message = _message, Data = _data };
        }

        public static ApiResponse<T> Failed(T _data, string _message = "Failed")
        {
            return new ApiResponse<T> { Code = ResponseCodes.Failed.code, Message = _message, Data = _data };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public static ApiResponse<T> NoRecordFound(string _message = "No record found")
        {
            return new ApiResponse<T> { Code = ResponseCodes.NotFound.code, Message = _message };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public static ApiResponse<T> BadRequest(string _message)
        {
            return new ApiResponse<T> { Code = ResponseCodes.BadRequest.code, Message = _message };
        }
    }
}