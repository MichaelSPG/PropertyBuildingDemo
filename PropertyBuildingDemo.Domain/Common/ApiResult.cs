using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PropertyBuildingDemo.Domain.Common
{
    public class ApiResult : IApiResult
    {
        public IEnumerable<string>  Message { get; set; }
        public bool                 Success { get; set; }
        public int                  ResultCode { get; set; } = -1;

        public bool IsSuccess()
        {
            return Success;
        }
        public bool Failed()
        {
            return !Success;
        }

        public static ApiResult SuccessResult(string InMessage)
        {
            return new ApiResult { Success = true, Message = new List<string> { InMessage } };
        }
        public static ApiResult SuccessResult()
        {
            return new ApiResult { Success = true };
        }

        public static ApiResult FailedResult(int InCode, string InMessage)
        {

            return new ApiResult { Success = false, Message = new List<string> { InMessage }, ResultCode = InCode };
        }
        public static ApiResult FailedResult(string InMessage)
        {
            return new ApiResult { Success = false, Message = new List<string> { InMessage } };
        }
        public static ApiResult FailedResult()
        {
            return new ApiResult { Success = false };
        }
    }

    public class ApiResult<TData> : ApiResult, IApiResult<TData>
    {
        public ApiResult() { }
        public TData Data { get; set; }

        public static ApiResult<TData> SuccessResult(TData data, string InMessage = null)
        {
            if(string.IsNullOrWhiteSpace(InMessage))
                return new ApiResult<TData> { Data = data, Success = true };


            return new ApiResult<TData> { Success = true, Data = data, Message = new List<string> { InMessage } };
        }
        public static ApiResult<TData> SuccessResult(string InMessage)
        {
            return new ApiResult<TData> { Success = true, Message = new List<string> { InMessage } };
        }
        public static ApiResult<TData> SuccessResult()
        {
            return new ApiResult<TData> { Success = true };
        }

        public static ApiResult<TData> FailedResult(int InCode, string InMessage)
        {

            return new ApiResult<TData> { Success = false, Message = new List<string> { InMessage }, ResultCode = InCode };
        }
        public static ApiResult<TData> FailedResult(string InMessage)
        {
            return new ApiResult<TData> { Success = false, Message = new List<string> { InMessage } };
        }
        public static ApiResult<TData> FailedResult()
        {
            return new ApiResult<TData> { Success = false };
        }
    }
}
