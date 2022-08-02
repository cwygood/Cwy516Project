using OpenTracing;
using OpenTracing.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Jaeger
{
    public class MyTracer : IDisposable
    {
        private readonly IScope _scope;
        public MyTracer(string name)
        {
            var tracer = GlobalTracer.Instance;
            ISpanBuilder builder = tracer.BuildSpan(name);
            //开始设置span
            _scope = builder.StartActive();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }

        public void SetOperationName(string operationName)
        {
            this._scope.Span.SetOperationName(operationName);
        }

        public void SetTag(string key, string value)
        {
            this._scope.Span.SetTag(key, value);
        }


    }
}
