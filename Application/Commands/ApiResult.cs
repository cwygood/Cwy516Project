using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands
{
    public class ApiResult
    {
        public static T OK<T>(string message, string requestId = "", object data = null) where T : BaseResponseCommand, new()
        {
            return new T()
            {
                IsSuccess = true,
                Code = "0",
                Messages = new List<string>() { message },
                Data = data,
                RequestId = requestId
            };
        }

        public static T Failure<T>(string message, string requestId = "") where T : BaseResponseCommand, new()
        {
            return new T()
            {
                IsSuccess = false,
                Code = "1",
                Messages = new List<string>() { message },
                RequestId = requestId
            };
        }
    }
}
