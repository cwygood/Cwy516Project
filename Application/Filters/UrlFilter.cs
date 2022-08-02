using Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Filters
{
    public class UrlFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //过滤url不符合规范的请求
            var url = context.HttpContext.Request.Path.Value;
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }
            if (!url.StartsWith("/api/"))
            {
                var result = new JsonResult(new BaseResponseCommand()
                {
                    Code = "1",
                    IsSuccess = false,
                    Messages = new List<string>() { "url格式错误！" }
                });
                context.Result = result;
            }
            return;
        }
    }
}
