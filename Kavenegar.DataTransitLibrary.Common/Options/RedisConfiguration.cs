using Kavenegar.DataTransitLibrary.Common.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Options
{
    public class RedisConfiguration : IRedisConfiguration
    {
        public string Connection { get; set; }
        public string InstanceName { get; set; }
    }
}
