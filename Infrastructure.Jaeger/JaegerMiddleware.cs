using Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Jaeger
{
    public class JaegerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<JaegerConfiguration> _options;
        private readonly ILogger<JaegerMiddleware> _logger;
        public JaegerMiddleware(RequestDelegate next, ILogger<JaegerMiddleware> logger, IOptionsMonitor<JaegerConfiguration> options)
        {
            this._options = options;
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ITracer tracer)
        {
            tracer = GlobalTracer.Instance;
            var path = context.Request.Path;
            if (Path.HasExtension(path))
            {
                await this._next(context).ConfigureAwait(false);
            }
            else
            {
                this._logger.LogInformation("jaeger调用");
                //接收传入的header
                var callingHeaders = new TextMapExtractAdapter(context.Request.Headers.ToDictionary(f => f.Key, f => f.Value.ToString()));
                var spanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, callingHeaders);
                ISpanBuilder builder = null;
                if (spanCtx != null)
                {
                    builder = tracer.BuildSpan(path).AsChildOf(spanCtx);
                }
                else
                {
                    builder = tracer.BuildSpan(path);
                }
                //开始设置span
                using (var scope = builder.StartActive())
                {
                    //记录请求信息到span
                    if (this._options.CurrentValue.IsQuerySpan)
                    {
                        foreach (var query in context.Request.Query)
                        {
                            //敏感词退出
                            if (this._options.CurrentValue.NoSpanKeys.Contains(query.Key))
                            {
                                continue;
                            }
                            var strQueryVal = Convert.ToString(query.Value);
                            var val = strQueryVal.Length > this._options.CurrentValue.QueryValueMaxLength ? strQueryVal.Substring(0, this._options.CurrentValue.QueryValueMaxLength) : strQueryVal;
                            scope.Span.SetTag(query.Key, val);
                        }
                    }
                    if (this._options.CurrentValue.IsFormSpan && context.Request.HasFormContentType)
                    {
                        foreach (var form in context.Request.Form)
                        {
                            //敏感词退出
                            if (this._options.CurrentValue.NoSpanKeys.Contains(form.Key))
                            {
                                continue;
                            }
                            var strFormVal = Convert.ToString(form.Value);
                            var val = strFormVal.Length > this._options.CurrentValue.FormValueMaxLength ? strFormVal.Substring(0, this._options.CurrentValue.FormValueMaxLength) : strFormVal;
                            scope.Span.SetTag(form.Key, val);
                        }
                    }
                    if (this._options.CurrentValue.IsBodySpan)
                    {
                        context.Request.EnableBuffering();
                        var requestReader = new StreamReader(context.Request.Body);

                        var requestContent = requestReader.ReadToEndAsync();
                        if (!string.IsNullOrWhiteSpace(requestContent.Result))
                        {
                            scope.Span.SetTag("body", requestContent.Result);
                        }
                        scope.Span.SetTag("body", requestContent.Result);
                        context.Request.Body.Position = 0;

                    }
                    await this._next(context).ConfigureAwait(false);
                }
            }
        }
    }
}
