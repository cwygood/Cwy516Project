using System;
using System.Collections.Generic;
using System.Text;
using Yitter.IdGenerator;

namespace Infrastructure.Common.SnowFlake
{
    public static class SnowFlake
    {
        public static long CreateSnowFlakeId()
        {
            return YitIdHelper.NextId();
        }
    }
}
