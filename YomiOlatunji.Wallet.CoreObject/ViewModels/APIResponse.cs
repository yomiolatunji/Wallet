using YomiOlatunji.Wallet.CoreObject.Enumerables;

namespace YomiOlatunji.Wallet.CoreObject.ViewModels
{
    public class APIResponse<T>
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
        public static APIResponse<T> Success(T _data, string _message = "Successful")
        {
            return new APIResponse<T> { Code = ResponseCodes.Success.code, Message = _message, Data = _data };
        }

        public static APIResponse<T> Failed(T _data, string _message = "Failed")
        {
            return new APIResponse<T> { Code = ResponseCodes.Failed.code, Message = _message, Data = _data };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public static APIResponse<T> NoRecordFound(string _message = "No record found")
        {
            return new APIResponse<T> { Code = ResponseCodes.NotFound.code, Message = _message };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public static APIResponse<T> BadRequest(string _message)
        {
            return new APIResponse<T> { Code = ResponseCodes.BadRequest.code, Message = _message };
        }
    }
}